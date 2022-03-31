using MusicBox.Modules.SheetEditor.Models;
using MusicBox.Services.Interfaces;
using MusicBox.Services.Interfaces.MusicSheetModels;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;

namespace MusicBox.Modules.SheetEditor.ViewModels
{
    public class SegmentEditorViewModel : BindableBase, ISegmentEditorViewModel
    {
        private const string DefaultName = "Segment1";
        private readonly IEventAggregator _eventAggregator;
        private readonly Func<IBarEditorViewModel> _barEditorVmCreator;
        private readonly IMidiPlayer _midiPlayer;
        private readonly Segment _segment;
        private string segmentName;

        public string SegmentName { get => segmentName; set => SetProperty(ref segmentName, value); }

        private ObservableCollection<IBarEditorViewModel> barEditorVms;
        public ObservableCollection<IBarEditorViewModel> BarEditorVms { get => barEditorVms; set => SetProperty(ref barEditorVms, value); }

        private IBarEditorViewModel selectedBarEditorVm;
        public IBarEditorViewModel SelectedBarEditorVm { get => selectedBarEditorVm; set => SetProperty(ref selectedBarEditorVm, value); }

        private bool isPlaying;
        public bool IsPlaying { get => isPlaying; set => SetProperty(ref isPlaying, value); }

        private int tempo;
        public int Tempo { get => tempo; set => SetProperty(ref tempo, value); }

        public DelegateCommand AddBarCommand { get; set; }
        public DelegateCommand DeleteBarCommand { get; set; }
        public DelegateCommand StartCommand { get; set; }
        public DelegateCommand StopCommand { get; set; }
        public DelegateCommand RewindToZeroCommand { get; set; }

        public SegmentEditorViewModel(Func<IBarEditorViewModel> barEditorVmCreator, IMidiPlayer midiPlayer, IEventAggregator evenAggregator)
        {
            _barEditorVmCreator = barEditorVmCreator;
            _midiPlayer = midiPlayer;
            _eventAggregator = evenAggregator;
            SegmentName = DefaultName;
            BarEditorVms = new ObservableCollection<IBarEditorViewModel>();
            AddBarCommand = new DelegateCommand(AddBar);
            DeleteBarCommand = new DelegateCommand(DeleteBar, CanDeleteBar).ObservesProperty(() => SelectedBarEditorVm);
            StartCommand = new DelegateCommand(StartPlaying, CanStartPlaying).ObservesProperty(() => IsPlaying);
            StopCommand = new DelegateCommand(StopPlaying, CanStopPlaying).ObservesProperty(() => IsPlaying);
            RewindToZeroCommand = new DelegateCommand(RewindToZero, CanRewindToZero);
            Tempo = 60;
            _eventAggregator.GetEvent<SelectedBarChanged>().Subscribe(OnSelectedBarChanged);
            _segment = new Segment();
            _midiPlayer.PlayingState += _midiPlayer_PlayingState;
            IsPlaying = false;
        }

        private void _midiPlayer_PlayingState(bool isPlaying)
        {
            IsPlaying = isPlaying;
        }

        private void StartPlaying()
        {
             ExtractSegmentInfo(_segment);
            _midiPlayer.PlaySegment(_segment, Tempo);
        }

        private bool CanStartPlaying()
        {
            return !IsPlaying && BarEditorVms.Count > 0;
        }

        private void StopPlaying()
        {
            _midiPlayer.Pause();
        }

        private bool CanStopPlaying()
        {
            return IsPlaying;
        }

        private void RewindToZero()
        {
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
            var newBarEditorVm = _barEditorVmCreator();
            BarEditorVms.Add(newBarEditorVm);
            SelectedBarEditorVm = newBarEditorVm;
            StartCommand.RaiseCanExecuteChanged();
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
                Bar currentBar = new Bar() { Id = barId, PlayOrder = barId };
                barEditorVm.ExtractBarInfo(currentBar);
                segment.Bars.Add(currentBar);
                barId++;
            }
            segment.Name = SegmentName;
        }
    }
}