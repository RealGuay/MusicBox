using MusicBox.Modules.SheetEditor.Models;
using MusicBox.Services.Interfaces;
using MusicBox.Services.Interfaces.MusicSheetModels;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MusicBox.Modules.SheetEditor.ViewModels
{
    public class SheetEditorViewModel : BindableBase
    {
        private IMidiPlayer _midiPlayer;
        private SheetInformation _sheetInformation;
        private readonly ISheetInformationRepo _sheetInfoRepo;
        private const int DefaultTempo = 60;

        #region Properties

        private bool isPlaying;
        private HandToPlay selectedHandToPlay;
        private int tempo;

        public SheetInformationViewModel SheetInformationVm { get; private set; }
        public SegmentCollectionViewModel SegmentCollectionVm { get; private set; }

        public bool IsSegmentSelected { get => SegmentCollectionVm.SelectedSegmentEditorVm != null; }

        public bool IsPlaying
        {
            get { return isPlaying; }
            set { SetProperty(ref isPlaying, value); }
        }

        public int Tempo { get => tempo; set => SetTempo(ref tempo, value); }

        private void SetTempo(ref int tempo, int value, [CallerMemberName] string propertyName = null)
        {
            if (SetProperty(ref tempo, value, propertyName))
            {
                ChangeMidiPlayerTempo(tempo);
            }
        }

        public List<SectionToPlay> SectionsToPlay { get; private set; }

        private SectionToPlay selectedSectionToPlay;

        public SectionToPlay SelectedSectionToPlay
        {
            get { return selectedSectionToPlay; }
            set { selectedSectionToPlay = value; }
        }

        public List<HandToPlay> HandsToPlay { get; private set; }

        public HandToPlay SelectedHandToPlay { get => selectedHandToPlay; set => selectedHandToPlay = value; }

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

            _midiPlayer.PlayingState += _midiPlayer_PlayingState;

            SegmentCollectionVm.PropertyChanged += SegmentCollectionVm_PropertyChanged;

            LoadCommand = new DelegateCommand(Load);
            SaveCommand = new DelegateCommand(Save, CanSave);
            PlayCommand = new DelegateCommand(Play, CanPlay);
            PauseCommand = new DelegateCommand(Pause);
            RewindCommand = new DelegateCommand(Rewind);

            SheetInformationVm.Tempo = DefaultTempo;
            Tempo = DefaultTempo;
            InitSectionsToPlay();
            InitHandsToPlay();
        }

        private void _midiPlayer_PlayingState(bool isPlaying)
        {
            IsPlaying = isPlaying;
        }

        private void InitHandsToPlay()
        {
            HandsToPlay = new List<HandToPlay> {
                new HandToPlay() { Hand=PlayingHand.Both,  Name="Both Hands"},
                new HandToPlay() { Hand=PlayingHand.Left,  Name="Left Hand"},
                new HandToPlay() { Hand=PlayingHand.Right, Name="Right Hand"},
            };
            SelectedHandToPlay = HandsToPlay.First();
        }

        private void InitSectionsToPlay()
        {
            SectionsToPlay = new List<SectionToPlay> {
                new SectionToPlay() { Section=Section.SHEET,   Name="Sheet"},
                new SectionToPlay() { Section=Section.SEGMENT, Name="Segment"},
                new SectionToPlay() { Section=Section.BAR,     Name="Bar"}
                };
            SelectedSectionToPlay = SectionsToPlay.First();
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
            if (!string.IsNullOrEmpty(_sheetInformation.Filename))
            {
                LoadSheetInfo();
            }
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
            _sheetInformation.Segments.Clear();
            if (SelectedSectionToPlay.Section == Section.SHEET)
            {
                ExtractAllSegments(SelectedHandToPlay.Hand);
            }
            else if (SelectedSectionToPlay.Section == Section.SEGMENT)
            {
                ExtractSelectedSegment(SelectedHandToPlay.Hand);
            }
            else
            {
                ExtractSelectedBar(SelectedHandToPlay.Hand);
            }
            _midiPlayer.PlaySheet(_sheetInformation, Tempo);   // tempo
            IsPlaying = true;
        }

        private bool CanPlay()
        {
            return !IsPlaying;
        }

        private void ExtractSheetInfo()
        {
            ExtractGeneralInfo();
            ExtractAllSegments(PlayingHand.Both);
        }

        private void ExtractGeneralInfo()
        {
            _sheetInformation.Title = SheetInformationVm.Title;
            _sheetInformation.LyricsBy = SheetInformationVm.LyricsBy;
            _sheetInformation.MusicBy = SheetInformationVm.MusicBy;
            _sheetInformation.Version = SheetInformationVm.Version;
            _sheetInformation.Filename = SheetInformationVm.Filename;
        }

        private void ExtractAllSegments(PlayingHand hand)
        {
            _sheetInformation.Segments.Clear();

            Dictionary<ISegmentEditorViewModel, Segment> processedSegments = new Dictionary<ISegmentEditorViewModel, Segment>();

            foreach (ISegmentEditorViewModel segmentEditorVm in SegmentCollectionVm.SegmentEditorVms)
            {
                Segment currentSegment;
                if (!processedSegments.ContainsKey(segmentEditorVm))
                {
                    currentSegment = ExtractOneSegment(segmentEditorVm, hand);
                    processedSegments.Add(segmentEditorVm, currentSegment);
                }
                else
                {
                    currentSegment = processedSegments[segmentEditorVm];
                }
                _sheetInformation.Segments.Add(currentSegment);
            }
        }

        private Segment ExtractOneSegment(ISegmentEditorViewModel segmentEditorVm, PlayingHand hand)
        {
            Segment segment = new Segment();
            segmentEditorVm.ExtractSegmentInfo(segment, hand);
            return segment;
        }

        private void ExtractSelectedSegment(PlayingHand hand)
        {
            Segment segment = ExtractOneSegment(SegmentCollectionVm.SelectedSegmentEditorVm, hand);
            _sheetInformation.Segments.Add(segment);
        }

        private void ExtractSelectedBar(PlayingHand hand)
        {
            Segment segment = new Segment();
            SegmentCollectionVm.SelectedSegmentEditorVm.ExtractSelectedBarInfo(segment, hand);
            _sheetInformation.Segments.Add(segment);
        }

        private void Pause()
        {
            _midiPlayer.Pause();
            IsPlaying = false;
        }

        private void Rewind()
        {
            _midiPlayer.RewindToZero();
        }

        private void ChangeMidiPlayerTempo(int tempo)
        {
            _midiPlayer.SetTempo(tempo);
        }

        #endregion Commands
    }
}