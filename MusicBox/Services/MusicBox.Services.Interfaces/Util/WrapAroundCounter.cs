using System;

namespace MusicBox.Services.Interfaces.Util
{
    public class WrapAroundCounter
    {
        public int CurrentValue { get; private set; }

        private int minValue;
        private int maxValue;
        private WrapAroundCounter parentCounter;
        private Action<int> countIncremented;

        public WrapAroundCounter(int minValue, int maxValue, WrapAroundCounter parentCounter, Action<int> countIncremented)
        {
            CurrentValue = minValue;
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.parentCounter = parentCounter;
            this.countIncremented = countIncremented;
        }

        public void Increment()
        {
            CurrentValue++;

            if (CurrentValue > maxValue)
            {
                CurrentValue = minValue;
                if (parentCounter != null)
                {
                    parentCounter.Increment();
                }
            }
            countIncremented?.Invoke(CurrentValue);
        }

        public void ResetCount()
        {
            CurrentValue = minValue;
        }
    }
}