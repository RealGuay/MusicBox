using System;

namespace MusicBox.Services.Interfaces.Util
{
    public class WrapAroundCounter
    {
        private int _minValue;
        private int _maxValue;
        private readonly Action<int> _countIncremented;
        private readonly Action _wrapAroundDetected;

        public int CurrentValue { get; private set; }

        public WrapAroundCounter(Action<int> countIncremented, Action wrapAroundDetected)
        {
            CurrentValue = 0;
            _minValue = 0;
            _maxValue = int.MaxValue;
            _countIncremented = countIncremented;
            _wrapAroundDetected = wrapAroundDetected;
        }

        public void Increment()
        {
            CurrentValue++;
            if (CurrentValue > _maxValue)
            {
                CurrentValue = _minValue;
                _wrapAroundDetected?.Invoke();
            }
            _countIncremented?.Invoke(CurrentValue);
        }

        public void SetRange(int min, int max)
        { 
            _minValue = min;
            _maxValue = max;
        }

        public void ResetCount()
        {
            CurrentValue = _minValue;
        }
    }
}