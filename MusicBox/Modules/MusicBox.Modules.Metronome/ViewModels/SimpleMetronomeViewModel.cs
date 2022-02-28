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
        public DelegateCommand RewindToZeroCommand { get; set; }

        private readonly IBeatMaker _beatMaker;

        private string timeSignatureName;

        public string TimeSignatureName
        {
            get { return timeSignatureName; }
            set {SetProperty(ref timeSignatureName , value); }
        }


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
            SetProperty(ref tempo, value, nameof(Tempo));  // do NOT forget to set the property name with nameof(...) 
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
            _beatMaker.BarReached += BarReached;
            _beatMaker.BeatReached += BeatReached;
            _beatMaker.SubBeatReached += SubBeatReached;
            _beatMaker.TickReached += TickReached;

            Tempo = 80;
            TimeSignature ts = TimeSignature.TS_12_8;
            TimeSignatureName = $"({ts.Name})";
            _beatMaker.SetParams(ts, Tempo, TickResolution.Normal);

            StartCommand = new DelegateCommand(Start);
            StopCommand = new DelegateCommand(Stop);
            RewindToZeroCommand = new DelegateCommand(RewindToZero);

            IsRunning = false;
        }

        private void BarReached(object sender, BarReachedEventArgs e)
        {
            BarCount = e.BarCount;
        }

        private void BeatReached(object sender, BeatReachedEventArgs e)
        {
            BeatCount = e.BeatCount;
        }

        private void SubBeatReached(object sender, SubBeatReachedEventArgs e)
        {
            SubBeatCount = e.SubBeatCount;
        }

        private void TickReached(object sender, TickReachedEventArgs e)
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

        private void RewindToZero()
        {
            _beatMaker.RewindToZero();
        }
    }
}