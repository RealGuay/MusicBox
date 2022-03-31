using MusicBox.Services.Interfaces;
using MusicBox.Services.Interfaces.MusicSheetModels;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Windows.Input;

namespace MusicBox.Modules.SheetEditor.ViewModels
{
    public class SheetEditorViewModel : BindableBase
    {
        private IMidiPlayer _midiPlayer;
        private SheetInformation _sheetInformation;

        #region Properties

        public SheetInformationViewModel SheetInformationVm { get; private set; }
        public SegmentCollectionViewModel SegmentCollectionVm { get; private set; }

        public bool IsSegmentSelected { get => SegmentCollectionVm.SelectedSegmentEditorVm != null; }

        public List<TimeSignature> TimeSignatures { get; set; }

        private bool isPlaying;

        public bool IsPlaying
        {
            get { return isPlaying; }
            set { SetProperty(ref isPlaying, value); }
        }

        #endregion Properties

        #region ICommands

        public ICommand LoadCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand PlayCommand { get; set; }
        public ICommand PauseCommand { get; set; }
        public ICommand RewindCommand { get; set; }

        #endregion ICommands

        public SheetEditorViewModel(IContainerProvider containerProvider)
        {
            SheetInformationVm = containerProvider.Resolve<SheetInformationViewModel>();
            SegmentCollectionVm = containerProvider.Resolve<SegmentCollectionViewModel>();
            _midiPlayer = containerProvider.Resolve<IMidiPlayer>();
            _sheetInformation = new SheetInformation(null); // sic Context

            SegmentCollectionVm.PropertyChanged += SegmentCollectionVm_PropertyChanged;

            LoadCommand = new DelegateCommand(Load);
            SaveCommand = new DelegateCommand(Save, CanSave);
            PlayCommand = new DelegateCommand(Play, CanPlay);
            PauseCommand = new DelegateCommand(Pause);
            RewindCommand = new DelegateCommand(Rewind);

            InitTimeSignatures();
            SheetInformationVm.Tempo = 60;
        }

        private void SegmentCollectionVm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (Equals(e.PropertyName, nameof(SegmentCollectionVm.SelectedSegmentEditorVm)))
            {
                RaisePropertyChanged(nameof(IsSegmentSelected));
            }
        }

        private void InitTimeSignatures()
        {
            TimeSignatures = new List<TimeSignature>
            {
                TimeSignature.TS_2_4,
                TimeSignature.TS_3_4,
                TimeSignature.TS_4_4,
                TimeSignature.TS_3_8,
                TimeSignature.TS_6_8,
                TimeSignature.TS_9_8,
                TimeSignature.TS_12_8
            };
            SheetInformationVm.TimeSignature = TimeSignature.TS_4_4;
        }

        #region Commands

        private void Load()
        {
            //SheetInformationVm.Title = $"{DateTime.Now}";
            //SheetInformationVm.LyricsBy = $"{DateTime.UtcNow}";
        }

        private void Save()
        {
        }

        private bool CanSave()
        {
            return true;
        }

        private void Play()
        {
            ExtractSheetInfo();
            _midiPlayer.PlaySheet(_sheetInformation, 60);   // tempo
            IsPlaying = true;
        }

        private bool CanPlay()
        {
            return !IsPlaying;
        }

        private void ExtractSheetInfo()
        {
            Dictionary<ISegmentEditorViewModel, Segment> processedSegments = new Dictionary<ISegmentEditorViewModel, Segment>();

            _sheetInformation.Segments.Clear();
            foreach (ISegmentEditorViewModel segmentEditorVm in SegmentCollectionVm.SegmentEditorVms)
            {
                Segment currentSegment;
                if (!processedSegments.ContainsKey(segmentEditorVm))
                {
                    currentSegment = new Segment();
                    segmentEditorVm.ExtractSegmentInfo(currentSegment);
                    processedSegments.Add(segmentEditorVm, currentSegment);
                }
                else
                {
                    currentSegment = processedSegments[segmentEditorVm];
                }
                _sheetInformation.Segments.Add(currentSegment);
            }
        }

        private void Pause()
        {
            IsPlaying = false;
        }

        private void Rewind()
        {
        }

        #endregion Commands
    }
}