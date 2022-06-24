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
        private readonly ISheetInformationRepo _sheetInfoRepo;

        #region Properties

        public SheetInformationViewModel SheetInformationVm { get; private set; }
        public SegmentCollectionViewModel SegmentCollectionVm { get; private set; }

        public bool IsSegmentSelected { get => SegmentCollectionVm.SelectedSegmentEditorVm != null; }

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
            _sheetInfoRepo = containerProvider.Resolve<ISheetInformationRepo>();

            SegmentCollectionVm.PropertyChanged += SegmentCollectionVm_PropertyChanged;

            LoadCommand = new DelegateCommand(Load);
            SaveCommand = new DelegateCommand(Save, CanSave);
            PlayCommand = new DelegateCommand(Play, CanPlay);
            PauseCommand = new DelegateCommand(Pause);
            RewindCommand = new DelegateCommand(Rewind);

            SheetInformationVm.Tempo = 60;
        }

        private void SegmentCollectionVm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (Equals(e.PropertyName, nameof(SegmentCollectionVm.SelectedSegmentEditorVm)))
            {
                RaisePropertyChanged(nameof(IsSegmentSelected));
            }
        }

        #region Commands

        private async void Load()
        {
            _sheetInformation = new SheetInformation(null);
            await _sheetInfoRepo.LoadAsync(_sheetInformation);
            LoadSheetInfo();
        }

        private void LoadSheetInfo()
        {
            LoadGeneralInfo();
            LoadSegments();
        }

        private void LoadGeneralInfo()
        {
            SheetInformationVm.Title = _sheetInformation.Title;
            SheetInformationVm.LyricsBy = _sheetInformation.LyricsBy;
            SheetInformationVm.MusicBy = _sheetInformation.MusicBy;
            SheetInformationVm.Version = _sheetInformation.Version;
            SheetInformationVm.Filename = _sheetInformation.Filename;
        }

        private void LoadSegments()
        {
            SegmentCollectionVm.LoadSegments(_sheetInformation.Segments);
        }

        private async void Save()
        {
            ExtractSheetInfo();
            await _sheetInfoRepo.SaveAsync(_sheetInformation);
            SheetInformationVm.Filename = _sheetInformation.Filename;
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
            ExtractGeneralInfo();
            ExtractSegments();
        }

        private void ExtractGeneralInfo()
        {
            _sheetInformation.Title = SheetInformationVm.Title;
            _sheetInformation.LyricsBy = SheetInformationVm.LyricsBy;
            _sheetInformation.MusicBy = SheetInformationVm.MusicBy;
            _sheetInformation.Version = SheetInformationVm.Version;
            _sheetInformation.Filename = SheetInformationVm.Filename;
        }

        private void ExtractSegments()
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