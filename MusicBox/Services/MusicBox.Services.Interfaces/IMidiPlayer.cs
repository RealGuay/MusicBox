using MusicBox.Services.Interfaces.MusicSheetModels;
using System;

namespace MusicBox.Services.Interfaces
{
    public interface IMidiPlayer
    {
        event Action<int, int> PlayingProgress;
        event Action<bool> PlayingState;

        void Pause();
        void PlaySegment(Segment segment, int tempo);
        void PlaySheet(SheetInformation sheetInfo, int tempo);
        void RewindToZero();
    }
}