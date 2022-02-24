using MusicBox.Services.Interfaces;
using MusicBox.Services.Interfaces.MusicSheetModels;
using MusicBox.Services.Interfaces.Util;
using MusicBox.Services.MidiInterfaces;
using System;

namespace MusicBox.Services
{
    public class BeatMaker : IBeatMaker
    {
        private readonly IMidiTimer _midiTimer;
        private TimeSignature _timeSignature;

        private int _tickPerSubBeat;

        private TickResolution _tickResolution;

        private int _currentBarCount;
        private int _currentBeatCount;
        private int _currentSubBeatCount;
        private int _absoluteTickCount;

        private WrapAroundCounter _barCounter;
        private WrapAroundCounter _beatCounter;
        private WrapAroundCounter _subBeatCounter;
        private WrapAroundCounter _tickCounter;

        public event EventHandler<BarReachedEventArgs> BarReached;

        public event EventHandler<BeatReachedEventArgs> BeatReached;

        public event EventHandler<SubBeatReachedEventArgs> SubBeatReached;

        public event EventHandler<TickReachedEventArgs> TickReached;

        public BeatMaker(IMidiTimer midiTimer)
        {
            _midiTimer = midiTimer;
            _midiTimer.TickDetected += TimerTickDetected;
            // SetParams(TimeSignature.TS_4_4, 60, TickResolution.Normal);  // default params
            SetParams(TimeSignature.TS_12_8, 60, TickResolution.Normal);  // default params
        }

        public void SetParams(TimeSignature signature, int tempo, TickResolution tickResolution)
        {
            _timeSignature = signature;
            _tickResolution = tickResolution;

            _tickPerSubBeat = (int)tickResolution * _timeSignature.NotesPerBeat / _timeSignature.NotesPerQuarter / _timeSignature.SubbeatsPerBeat;

            ResetAllCounters();
            InitTimer(tempo);
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
            double beatsPerQuarter = _timeSignature.NotesPerQuarter / _timeSignature.NotesPerBeat; // beat/Quarter
            double msPerQuarter = beatsPerQuarter * msPerBeat; // ms/Quarter
            double msPerTick = msPerQuarter / (int)_tickResolution; // ms/tick
            return (int)msPerTick;
        }

        public void ResetAllCounters()
        {
            _currentBarCount = 1;
            _currentBeatCount = 1;
            _currentSubBeatCount = 1;
            _absoluteTickCount = 0;

            // todo reset counts and avoid creation of new counters
            _barCounter = new WrapAroundCounter(_currentBarCount, int.MaxValue, null, BarIncremented);
            _beatCounter = new WrapAroundCounter(_currentBeatCount, _timeSignature.BeatsPerBar, _barCounter, BeatIncremented);
            _subBeatCounter = new WrapAroundCounter(_currentSubBeatCount, _timeSignature.SubbeatsPerBeat, _beatCounter, SubBeatIncremented);
            _tickCounter = new WrapAroundCounter(0, _tickPerSubBeat - 1, _subBeatCounter, null);

            _barCounter.ResetCount();
            _beatCounter.ResetCount();
            _subBeatCounter.ResetCount();
            _tickCounter.ResetCount();
        }

        public void RewindToStart()
        {
            ResetAllCounters();
        }

        private void TimerTickDetected(object sender, TickEventArgs e)
        {
            _absoluteTickCount++;
            _tickCounter.Increment();
            TickReached?.Invoke(this, new TickReachedEventArgs() { TickInSubBeatCount = _tickCounter.CurrentValue, AbsoluteTickCount = _absoluteTickCount });
        }

        private void BarIncremented(int barCount)
        {
            BarReached?.Invoke(this, new BarReachedEventArgs() { BarCount = barCount });
        }

        private void BeatIncremented(int beatCount)
        {
            //            Debug.WriteLine($"BeatReached {beatCount}");
            BeatReached?.Invoke(this, new BeatReachedEventArgs() { BeatCount = beatCount });
        }

        private void SubBeatIncremented(int subBeatCount)
        {
            //Debug.WriteLine($"SubBeatReached {subBeatCount}");
            SubBeatReached?.Invoke(this, new SubBeatReachedEventArgs() { SubBeatCount = subBeatCount });
            UpdateCurrentCounts();
        }

        private void UpdateCurrentCounts()
        {
            _currentBarCount = _barCounter.CurrentValue;
            _currentBeatCount = _beatCounter.CurrentValue;
            _currentSubBeatCount = _subBeatCounter.CurrentValue;
        }

        public void Start()
        {
            _midiTimer.Start();
        }

        public void Stop()
        {
            _midiTimer.Stop();
        }

        public void Dispose()
        {
            _midiTimer?.Dispose();
        }
    }
}