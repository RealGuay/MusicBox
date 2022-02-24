using MusicBox.Services.MidiInterfaces;
using Sanford.Multimedia.Timers;
using System;

namespace MusicBox.Services.MidiSanford
{
    public class MidiTimer : IMidiTimer
    {
        private const int DEFAULT_PERIOD = 100;  // 10 ms
        private readonly ITimer _timer;
        private int _tickCount;

        private int period;

        public int Period
        {
            get { return period; }
            set { ChangePeriod(ref period, value); }
        }

        public event EventHandler<TickEventArgs> TickDetected;

        public MidiTimer()
        {
            _timer = TimerFactory.Create();
            Period = DEFAULT_PERIOD;
            _timer.Tick += OnTimerTick;
        }

        public void Start()
        {
            if (!_timer.IsRunning)
            {
                _tickCount = 0;
                _timer.Start();
            }
        }

        public void Stop()
        {
            if (_timer.IsRunning)
            {
                _timer.Stop();
            }
        }

        public void Dispose()
        {
            Stop();
            _timer?.Dispose();
        }

        private void ChangePeriod(ref int period, int value)
        {
            if (period != value)
            {
                period = value <= 0 ? DEFAULT_PERIOD : value;
                _timer.Period = period;
            }
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            _tickCount++;
            TickDetected?.Invoke(this, new TickEventArgs { TickCountFromStart = _tickCount });
        }
    }
}