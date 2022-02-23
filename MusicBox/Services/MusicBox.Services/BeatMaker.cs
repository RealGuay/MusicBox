using MusicBox.Services.Interfaces;
using MusicBox.Services.Interfaces.MusicSheetModels;
using MusicBox.Services.Interfaces.Util;
using MusicBox.Services.MidiInterfaces;
using System;

namespace MusicBox.Services
{
    public class BeatMaker : IBeatMaker
    {
        private IMidiTimer _midiTimer;
        private TimeSignature _timeSignature;

        private int _typeOfNote;
        private int _timerPeriodMs;

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
        private const int TICK_PER_BEAT = 24;

        private int _subBeatsPerBeat;

        private DateTime _lastTick;
        private int _beatsPerBar;

        public event EventHandler<BarReachedEventArgs> BarReached;

        public event EventHandler<BeatReachedEventArgs> BeatReached;

        public event EventHandler<SubBeatReachedEventArgs> SubBeatReached;

        public event EventHandler<TickReachedEventArgs> TickReached;

        //public BeatMaker(int tempo, int beatsPerBar, int typeOfNote, int subBeatsPerBeat)
        //{
        //   TimeSignature ts = TimeSignature.CreateTimeSignature4_4();
        //   int tickResolution = 96;

        //   BeatMaker(ts, tempo, tickResolution);

        //   this.beatsPerBar = beatsPerBar;
        //   this.typeOfNote = typeOfNote;
        //   this.subBeatsPerBeat = subBeatsPerBeat;

        //   tickPerSubBeat = TICK_PER_BEAT / subBeatsPerBeat;

        //   ResetAllCounts();

        //   lastTick = DateTime.Now;

        //   timerPeriodMs = CalculateTimerPeriod(tempo);
        //   InitTimer(timerPeriodMs);
        //}

        public BeatMaker(IMidiTimer midiTimer)
        {
            _midiTimer = midiTimer;
            _midiTimer.TickDetected += TimerTickDetected;
        }

        public void Init(TimeSignature signature, int tempo, TickResolution tickResolution)
        {
            _timeSignature = signature;
            _tickResolution = tickResolution;

            _beatsPerBar = signature.BeatsPerBar;
            _typeOfNote = signature.TypeOfNote;
            _subBeatsPerBeat = signature.SubbeatsPerBeat;

            _tickPerSubBeat = (int)tickResolution / _typeOfNote / _subBeatsPerBeat;

            ResetAllCounts();
            _lastTick = DateTime.Now;
            InitTimer(tempo);
        }

        public void ResetAllCounts()
        {
            _currentBarCount = 1;
            _currentBeatCount = 1;
            _currentSubBeatCount = 1;
            _absoluteTickCount = 0;

            // todo reset counts and avoid creation of new counters
            _barCounter = new WrapAroundCounter(_currentBarCount, int.MaxValue, null, BarIncremented);
            _beatCounter = new WrapAroundCounter(_currentBeatCount, _beatsPerBar, _barCounter, BeatIncremented);
            _subBeatCounter = new WrapAroundCounter(_currentSubBeatCount, _subBeatsPerBeat, _beatCounter, SubBeatIncremented);
            _tickCounter = new WrapAroundCounter(0, _tickPerSubBeat - 1, _subBeatCounter, null);

            _barCounter.ResetCount();
            _beatCounter.ResetCount();
            _subBeatCounter.ResetCount();
            _tickCounter.ResetCount();
        }

        private void InitTimer(int tempo)
        {
            _midiTimer.Stop();
            SetTempo(tempo);
        }

        public void SetTempo(int newTempo)
        {
            _timerPeriodMs = CalculateTimerPeriod(newTempo);
            _midiTimer.Period = _timerPeriodMs;
        }

        private int CalculateTimerPeriod(int tempo)
        {
            double beatPeriodMs = 60.0 * 1000.0 / tempo;
            double tickPeriodMs = beatPeriodMs * _timeSignature.TypeOfNote / (int)_tickResolution / _timeSignature.NotesPerBeat;
            return (int)tickPeriodMs;
        }

        public void RewindToStart()
        {
            ResetAllCounts();
        }

        private void TimerTickDetected(object sender, TickEventArgs e)
        {
            TickReached(this, new TickReachedEventArgs() { TickInBarCount = _tickCounter.CurrentValue, AbsoluteTickCount = _absoluteTickCount });
            _tickCounter.Increment();
            _absoluteTickCount++;
            TraceTime();
        }

        private void BarIncremented(int barCount)
        {
            BarReached(this, new BarReachedEventArgs() { BarCount = barCount });
        }

        private void BeatIncremented(int beatCount)
        {
            //            Debug.WriteLine($"BeatReached {beatCount}");
            BeatReached(this, new BeatReachedEventArgs() { BeatCount = beatCount });
        }

        private void SubBeatIncremented(int subBeatCount)
        {
            //Debug.WriteLine($"SubBeatReached {subBeatCount}");
            SubBeatReached(this, new SubBeatReachedEventArgs() { SubBeatCount = subBeatCount });
            UpdateCurrentCounts();
        }

        private void TraceTime()
        {
            DateTime now = DateTime.Now;
            TimeSpan diffTime = now - _lastTick;
            _lastTick = now;
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