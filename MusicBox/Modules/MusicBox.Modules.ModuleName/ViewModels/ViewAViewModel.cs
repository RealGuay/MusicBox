using MusicBox.Core.Mvvm;
using MusicBox.Services.Interfaces;
using Prism.Commands;
using Prism.Regions;
using System;

namespace MusicBox.Modules.ModuleName.ViewModels
{
    public class ViewAViewModel : RegionViewModelBase
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        private readonly IBeatMaker _beatMaker;

        private string barProgress;

        public string BarProgress
        {
            get { return barProgress; }
            set { SetProperty(ref barProgress, value); }
        }

        private string beatProgress;

        public string BeatProgress
        {
            get { return beatProgress; }
            set { SetProperty(ref beatProgress, value); }
        }

        private string subbeatProgress;

        public string SubbeatProgress
        {
            get { return subbeatProgress; }
            set { SetProperty(ref subbeatProgress, value); }
        }

        private string tickProgress;

        public string TickProgress
        {
            get { return tickProgress; }
            set { SetProperty(ref tickProgress, value); }
        }



        public DelegateCommand StartCommand { get; private set; }


        // todo remove IBeatMaker in ctor
        public ViewAViewModel(IRegionManager regionManager, IMessageService messageService, IBeatMaker beatMaker) :
            base(regionManager)
        {
            Message = messageService.GetMessage();

            BeatProgress = "Not running";
            StartCommand = new DelegateCommand(StartTimer);

            _beatMaker = beatMaker;
            _beatMaker.SubBeatReached += _beatMaker_SubBeatReached;
            _beatMaker.BeatReached += _beatMaker_BeatReached;
            _beatMaker.BarReached += _beatMaker_BarReached;
            _beatMaker.TickReached += _beatMaker_TickReached;
        }

        private void _beatMaker_TickReached(object sender, Services.Interfaces.MusicSheetModels.TickReachedEventArgs e)
        {
            TickProgress = $"TickInSubBeat : {e.TickInSubBeatCount} AbsoluteTick: {e.AbsoluteTickCount}";
            if (e.AbsoluteTickCount >= 2400)
            {
                _beatMaker.Stop();
            }
        }

        private void _beatMaker_SubBeatReached(object sender, Services.Interfaces.MusicSheetModels.SubBeatReachedEventArgs e)
        {
            SubbeatProgress = $"SubBeat reached: {e.SubBeatCount}";
        }

        private void _beatMaker_BarReached(object sender, Services.Interfaces.MusicSheetModels.BarReachedEventArgs e)
        {
            BarProgress = $"Bar reached: {e.BarCount}";
        }

        private void StartTimer()
        {
            _beatMaker.Start();
        }

        private void _beatMaker_BeatReached(object sender, Services.Interfaces.MusicSheetModels.BeatReachedEventArgs e)
        {
            BeatProgress = $"Beat reached: {e.BeatCount}";
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            //do something
        }
    }
}
