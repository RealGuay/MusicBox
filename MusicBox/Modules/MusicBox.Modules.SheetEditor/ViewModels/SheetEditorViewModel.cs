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
        #region Properties

        public SheetInformationViewModel SheetInformationVm { get; private set; }

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
            LoadCommand = new DelegateCommand(Load);
            SaveCommand = new DelegateCommand(Save, CanSave);
            PlayCommand = new DelegateCommand(Play, CanPlay);
            PauseCommand = new DelegateCommand(Pause);
            RewindCommand = new DelegateCommand(Rewind);

            InitTimeSignatures();
            SheetInformationVm.Tempo = 60;
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
            IsPlaying = true;
        }

        private bool CanPlay()
        {
            return !IsPlaying;
        }

        private void Pause()
        {
            IsPlaying=false;
        }

        private void Rewind()
        {
        }

        #endregion Commands
    }
}