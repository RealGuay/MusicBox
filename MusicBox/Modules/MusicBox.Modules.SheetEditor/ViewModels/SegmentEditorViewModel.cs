using MusicBox.Modules.SheetEditor.Models;
using MusicBox.Services.Interfaces;
using MusicBox.Services.Interfaces.MusicSheetModels;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MusicBox.Modules.SheetEditor.ViewModels
{
    public class SegmentEditorViewModel : BindableBase, ISegmentEditorViewModel
    {
        private const string DefaultName = "Segment1";
        private readonly IEventAggregator _eventAggregator;
        private readonly Func<TimeSignature, KeySignature, IBarEditorViewModel> _barEditorVmCreator;
        private readonly IMidiPlayer _midiPlayer;
        private readonly Segment _segment;
        private string segmentName;

        public string SegmentName { get => segmentName; set => SetProperty(ref segmentName, value); }

        private ObservableCollection<IBarEditorViewModel> barEditorVms;
        public ObservableCollection<IBarEditorViewModel> BarEditorVms { get => barEditorVms; set => SetProperty(ref barEditorVms, value); }

        private IBarEditorViewModel selectedBarEditorVm;
        public IBarEditorViewModel SelectedBarEditorVm { get => selectedBarEditorVm; set => SetProperty(ref selectedBarEditorVm, value); }

        public List<TimeSignature> TimeSignatures { get; set; }

        private TimeSignature selectedTimeSignature;
        public TimeSignature SelectedTimeSignature { get => selectedTimeSignature; set => SetProperty(ref selectedTimeSignature, value); }

        public List<KeySignature> KeySignatures { get; private set; }

        private KeySignature selectedKeySignature;
        public KeySignature SelectedKeySignature { get => selectedKeySignature; set => SetProperty(ref selectedKeySignature , value); }

        private bool isPlaying;
        public bool IsPlaying { get => isPlaying; set => SetProperty(ref isPlaying, value); }

        private int tempo;
        public int Tempo { get => tempo; set => SetProperty(ref tempo, value); }

        public bool IsSegmentEmpty { get => BarEditorVms.Count == 0; }

        public DelegateCommand AddBarCommand { get; set; }
        public DelegateCommand DeleteBarCommand { get; set; }
        public DelegateCommand PlayCommand { get; set; }
        public DelegateCommand PauseCommand { get; set; }
        public DelegateCommand RewindToZeroCommand { get; set; }

        public SegmentEditorViewModel(Func<TimeSignature, KeySignature, IBarEditorViewModel> barEditorVmCreator, IMidiPlayer midiPlayer, IEventAggregator evenAggregator)
        {
            _barEditorVmCreator = barEditorVmCreator;
            _midiPlayer = midiPlayer;
            _eventAggregator = evenAggregator;
            SegmentName = DefaultName;
            BarEditorVms = new ObservableCollection<IBarEditorViewModel>();
            AddBarCommand = new DelegateCommand(AddBar);
            DeleteBarCommand = new DelegateCommand(DeleteBar, CanDeleteBar).ObservesProperty(() => SelectedBarEditorVm);
            PlayCommand = new DelegateCommand(Play, CanPlay).ObservesProperty(() => IsPlaying);
            PauseCommand = new DelegateCommand(Pause, CanPause).ObservesProperty(() => IsPlaying);
            RewindToZeroCommand = new DelegateCommand(RewindToZero, CanRewindToZero).ObservesProperty(() => IsPlaying); ;
            Tempo = 60;
            _eventAggregator.GetEvent<SelectedBarChanged>().Subscribe(OnSelectedBarChanged);
            _segment = new Segment();
            _midiPlayer.PlayingState += _midiPlayer_PlayingState;
            IsPlaying = false;
            InitSignatures();
        }

        private void InitSignatures()
        {
            KeySignatures = KeySignature.AllKeySignatures;
            SelectedKeySignature = KeySignature.Natural;
            TimeSignatures = TimeSignature.AllTimeSignatures;
            SelectedTimeSignature = TimeSignature.TS_4_4;
        }

        private void _midiPlayer_PlayingState(bool isPlaying)
        {
            IsPlaying = isPlaying;
        }

        private void Play()
        {
            ExtractSegmentInfo(_segment);
            _midiPlayer.PlaySegment(_segment, Tempo);
        }

        private bool CanPlay()
        {
            return !IsPlaying && BarEditorVms.Count > 0;
        }

        private void Pause()
        {
            _midiPlayer.Pause();
        }

        private bool CanPause()
        {
            return IsPlaying;
        }

        private void RewindToZero()
        {
            _midiPlayer.RewindToZero();
        }

        private bool CanRewindToZero()
        {
            return !IsPlaying && BarEditorVms.Count > 0;
        }

        private void OnSelectedBarChanged(IBarEditorViewModel newSelectedBarVm)
        {
            if (SelectedBarEditorVm != newSelectedBarVm)
            {
                SelectedBarEditorVm = newSelectedBarVm;
            }
        }

        private void AddBar()
        {
            var newBarEditorVm = _barEditorVmCreator(SelectedTimeSignature, SelectedKeySignature);
            BarEditorVms.Add(newBarEditorVm);
            SelectedBarEditorVm = newBarEditorVm;
            PlayCommand.RaiseCanExecuteChanged();
            RaisePropertyChanged(nameof(IsSegmentEmpty));
        }

        private void DeleteBar()
        {
            int removedIndex = BarEditorVms.IndexOf(SelectedBarEditorVm);

            BarEditorVms.RemoveAt(removedIndex);

            int newSelectedIndex = removedIndex < BarEditorVms.Count ? removedIndex : removedIndex - 1;
            if (newSelectedIndex > -1)
            {
                SelectedBarEditorVm = BarEditorVms[newSelectedIndex];
            }
            DeleteBarCommand?.RaiseCanExecuteChanged();
            RaisePropertyChanged(nameof(IsSegmentEmpty));
        }

        private bool CanDeleteBar()
        {
            return SelectedBarEditorVm != null;
        }

        public ISegmentEditorViewModel DeepCopy()
        {
            SegmentEditorViewModel copy = new SegmentEditorViewModel(_barEditorVmCreator, _midiPlayer, _eventAggregator)
            {
                SegmentName = SegmentName
            };

            foreach (BarEditorViewModel barEditorViewModel in BarEditorVms)
            {
                IBarEditorViewModel newBarEditorVm = barEditorViewModel.DeepCopy();
                copy.BarEditorVms.Add(newBarEditorVm);
            }
            return copy;
        }

        public void ExtractSegmentInfo(Segment segment)
        {
            int barId = 0;
            segment.Bars.Clear();
            foreach (BarEditorViewModel barEditorVm in BarEditorVms)
            {
                Bar currentBar = new Bar() { Id = barId, PlayOrder = barId};
                barEditorVm.ExtractBarInfo(currentBar);
                segment.Bars.Add(currentBar);
                barId++;
            }
            segment.Name = SegmentName;
            segment.TimeSignature = SelectedTimeSignature;
            segment.KeySignature = SelectedKeySignature;
        }

        public void LoadSegmentInfo(Segment segment)
        {
            SegmentName = segment.Name;
            SelectedTimeSignature = segment.TimeSignature;
            SelectedKeySignature = segment.KeySignature;    

            foreach (var bar in segment.Bars)
            {
                var newBarEditorVm = _barEditorVmCreator(SelectedTimeSignature, SelectedKeySignature); 
                newBarEditorVm.LoadBarInfo(bar);
                BarEditorVms.Add(newBarEditorVm);
            }
            SelectedBarEditorVm = BarEditorVms.Count > 0 ? BarEditorVms[0] : null;
        }
    }
}