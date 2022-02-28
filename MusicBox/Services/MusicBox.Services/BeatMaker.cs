using MusicBox.Services.Interfaces;
using MusicBox.Services.Interfaces.MusicSheetModels;
using MusicBox.Services.Interfaces.Util;
using MusicBox.Services.MidiInterfaces;
using Prism.Ioc;
using System;

namespace MusicBox.Services
{
    public class BeatMaker : IBeatMaker
    {
        private readonly IMidiTimer _midiTimer;
        private readonly IContainerProvider _containerProvider;
        private TimeSignature _timeSignature;
        private TickResolution _tickResolution;
        private int _ticksFromZero;

        private WrapAroundCounter _barCounter;
        private WrapAroundCounter _beatCounter;
        private WrapAroundCounter _subBeatCounter;
        private WrapAroundCounter _tickCounter;

        public event EventHandler<BarReachedEventArgs> BarReached;

        public event EventHandler<BeatReachedEventArgs> BeatReached;

        public event EventHandler<SubBeatReachedEventArgs> SubBeatReached;

        public event EventHandler<TickReachedEventArgs> TickReached;

        public BeatMaker(IMidiTimer midiTimer, IContainerProvider containerProvider)
        {
            _midiTimer = midiTimer;
            _midiTimer.TickDetected += TimerTickDetected;

            _containerProvider = containerProvider;

            CreateAllCounters();
            SetParams(TimeSignature.TS_4_4, 30, TickResolution.Normal);  // default params
        }

        private void CreateAllCounters()
        {
            var createWac = _containerProvider.Resolve<Func<Action<int>, Action, WrapAroundCounter>>();

            _barCounter = createWac(BarIncremented, delegate { });
            _beatCounter = createWac(BeatIncremented, () => _barCounter.Increment());
            _subBeatCounter = createWac(SubBeatIncremented, () => _beatCounter.Increment());
            _tickCounter = createWac(delegate { }, () => _subBeatCounter.Increment());
        }

        public void SetParams(TimeSignature signature, int tempo, TickResolution tickResolution)
        {
            _timeSignature = signature;
            _tickResolution = tickResolution;

            SetRangeAllCounters();
            ResetAllCounters();
            InitTimer(tempo);
        }

        private void SetRangeAllCounters()
        {
            int tickPerSubBeat = (int)_tickResolution * _timeSignature.NotesPerBeat / _timeSignature.NotesPerQuarter / _timeSignature.SubbeatsPerBeat;

            _barCounter.SetRange(1, int.MaxValue);
            _beatCounter.SetRange(1, _timeSignature.BeatsPerBar);
            _subBeatCounter.SetRange(1, _timeSignature.SubbeatsPerBeat);
            _tickCounter.SetRange(0, tickPerSubBeat - 1);
        }

        private void ResetAllCounters()
        {
            _ticksFromZero = 0;
            _barCounter.ResetCount();
            _beatCounter.ResetCount();
            _subBeatCounter.ResetCount();
            _tickCounter.ResetCount();

            TransmitAllCounts();
        }

        private void TransmitAllCounts()
        {
            BarReached?.Invoke(this, new BarReachedEventArgs() { BarCount = _barCounter.CurrentValue });
            BeatReached?.Invoke(this, new BeatReachedEventArgs() { BeatCount = _beatCounter.CurrentValue });
            SubBeatReached?.Invoke(this, new SubBeatReachedEventArgs() { SubBeatCount = _subBeatCounter.CurrentValue });
            TickReached?.Invoke(this, new TickReachedEventArgs() { TickInSubBeatCount = _tickCounter.CurrentValue, TicksFromZero = _ticksFromZero });
        }

        private void InitTimer(int tempo)
        {
            _midiTimer.Stop();
            SetTempo(tempo);
        }

        public void SetTempo(int newTempo)
        {
            int timerPeriodMs = CalculateTimerPeriod(newTempo);
            _midiTimer.Period = timerPeriodMs;
        }

        private int CalculateTimerPeriod(int tempo)
        {
            double msPerBeat = 60.0 * 1000.0 / tempo; // ms/beat
            double beatsPerQuarter = (double)_timeSignature.NotesPerQuarter / _timeSignature.NotesPerBeat; // beat/Quarter
            double msPerQuarter = beatsPerQuarter * msPerBeat; // ms/Quarter
            double msPerTick = msPerQuarter / (int)_tickResolution; // ms/tick
            return (int)msPerTick;
        }

        private void TimerTickDetected(object sender, TickEventArgs e)
        {
            _ticksFromZero++;
            _tickCounter.Increment();
            TickReached?.Invoke(this, new TickReachedEventArgs() { TickInSubBeatCount = _tickCounter.CurrentValue, TicksFromZero = _ticksFromZero });
        }

        private void SubBeatIncremented(int subBeatCount)
        {
            SubBeatReached?.Invoke(this, new SubBeatReachedEventArgs() { SubBeatCount = subBeatCount });
        }

        private void BeatIncremented(int beatCount)
        {
            BeatReached?.Invoke(this, new BeatReachedEventArgs() { BeatCount = beatCount });
        }

        private void BarIncremented(int barCount)
        {
            BarReached?.Invoke(this, new BarReachedEventArgs() { BarCount = barCount });
        }

        public void Start()
        {
            TransmitAllCounts();
            _midiTimer.Start();
        }

        public void Stop()
        {
            _midiTimer.Stop();
        }

        public void RewindToZero()
        {
            ResetAllCounters();
        }

        public void Dispose()
        {
            _midiTimer?.Dispose();
        }
    }
}