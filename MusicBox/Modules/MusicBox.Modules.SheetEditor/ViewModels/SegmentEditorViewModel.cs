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
        public KeySignature SelectedKeySignature { get => selectedKeySignature; set => SetProperty(ref selectedKeySignature, value); }

        public bool IsSegmentEmpty { get => BarEditorVms.Count == 0; }

        public DelegateCommand AddBarCommand { get; set; }
        public DelegateCommand DeleteBarCommand { get; set; }

        public SegmentEditorViewModel(Func<TimeSignature, KeySignature, IBarEditorViewModel> barEditorVmCreator, IEventAggregator evenAggregator)
        {
            _barEditorVmCreator = barEditorVmCreator;
            _eventAggregator = evenAggregator;
            SegmentName = DefaultName;
            BarEditorVms = new ObservableCollection<IBarEditorViewModel>();
            AddBarCommand = new DelegateCommand(AddBar);
            DeleteBarCommand = new DelegateCommand(DeleteBar, CanDeleteBar).ObservesProperty(() => SelectedBarEditorVm);
            _eventAggregator.GetEvent<SelectedBarChanged>().Subscribe(OnSelectedBarChanged);
            _segment = new Segment();
            InitSignatures();
        }

        private void InitSignatures()
        {
            KeySignatures = KeySignature.AllKeySignatures;
            SelectedKeySignature = KeySignature.Natural;
            TimeSignatures = TimeSignature.AllTimeSignatures;
            SelectedTimeSignature = TimeSignature.TS_4_4;
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
            SegmentEditorViewModel copy = new SegmentEditorViewModel(_barEditorVmCreator, _eventAggregator)
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

        public void ExtractSegmentInfo(Segment segment, PlayingHand hand)
        {
            int barId = 0;
            segment.Bars.Clear();
            foreach (BarEditorViewModel barEditorVm in BarEditorVms)
            {
                ExtractBarInfo(segment, barId, barEditorVm, hand);
                barId++;
            }
            segment.Name = SegmentName;
            segment.TimeSignature = SelectedTimeSignature;
            segment.KeySignature = SelectedKeySignature;
        }

        private void ExtractBarInfo(Segment segment, int barId, IBarEditorViewModel barEditorVm, PlayingHand hand)
        {
            Bar currentBar = new Bar() { Id = barId, PlayOrder = barId };
            barEditorVm.ExtractBarInfo(currentBar, hand);
            segment.Bars.Add(currentBar);
        }

        public void ExtractSelectedBarInfo(Segment segment, PlayingHand hand)
        {
            segment.Bars.Clear();
            ExtractBarInfo(segment, 0, SelectedBarEditorVm, hand);
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