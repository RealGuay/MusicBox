using MusicBox.Services.Interfaces;
using MusicBox.Services.Interfaces.MusicSheetModels;
using Prism.Commands;
using Prism.Mvvm;

namespace MusicBox.Modules.Metronome.ViewModels
{
    public class SimpleMetronomeViewModel : BindableBase
    {
        public DelegateCommand StartCommand { get; set; }
        public DelegateCommand StopCommand { get; set; }
        public DelegateCommand ResetCountCommand { get; set; }

        private readonly IBeatMaker _beatMaker;

        private int tempo;

        public int Tempo
        {
            get { return tempo; }
            set
            {
                ChangeTempo(value);
            }
        }

        private void ChangeTempo(int value)
        {
            SetProperty(ref tempo, value, nameof(Tempo));
            _beatMaker.SetTempo(tempo);
        }

        private bool isRunning;

        public bool IsRunning
        {
            get { return isRunning; }
            set { SetProperty(ref isRunning, value); }
        }

        private int barCount;

        public int BarCount
        {
            get { return barCount; }
            set { SetProperty(ref barCount, value); }
        }

        private int beatCount;

        public int BeatCount
        {
            get { return beatCount; }
            set { SetProperty(ref beatCount, value); }
        }

        private int subBeatCount;

        public int SubBeatCount
        {
            get { return subBeatCount; }
            set { SetProperty(ref subBeatCount, value); }
        }

        private int tickCount;

        public int TickCount
        {
            get { return tickCount; }
            set { SetProperty(ref tickCount, value); }
        }

        public SimpleMetronomeViewModel(IBeatMaker beatMaker)
        {
            _beatMaker = beatMaker;
            _beatMaker.BarReached += _beatMaker_BarReached;
            _beatMaker.BeatReached += _beatMaker_BeatReached;
            _beatMaker.SubBeatReached += _beatMaker_SubBeatReached;
            _beatMaker.TickReached += _beatMaker_TickReached;

            Tempo = 60;
            _beatMaker.SetParams(Services.Interfaces.MusicSheetModels.TimeSignature.TS_12_8, Tempo, TickResolution.Normal);

            StartCommand = new DelegateCommand(Start);
            StopCommand = new DelegateCommand(Stop);
            ResetCountCommand = new DelegateCommand(ResetCount);

            IsRunning = false;
        }

        private void _beatMaker_BarReached(object sender, BarReachedEventArgs e)
        {
            BarCount = e.BarCount;
        }

        private void _beatMaker_BeatReached(object sender, BeatReachedEventArgs e)
        {
            BeatCount = e.BeatCount;
        }

        private void _beatMaker_SubBeatReached(object sender, SubBeatReachedEventArgs e)
        {
            SubBeatCount = e.SubBeatCount;
        }

        private void _beatMaker_TickReached(object sender, TickReachedEventArgs e)
        {
            TickCount = e.TickInSubBeatCount;
        }

        private void Start()
        {
            _beatMaker.Start();
            IsRunning = true;
        }

        private void Stop()
        {
            _beatMaker.Stop();
            IsRunning = false;
        }

        private void ResetCount()
        {
            _beatMaker.ResetAllCounters();
        }
    }
}