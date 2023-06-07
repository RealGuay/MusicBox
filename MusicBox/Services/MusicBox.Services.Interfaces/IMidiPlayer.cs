using MusicBox.Services.Interfaces.MusicSheetModels;
using System;

namespace MusicBox.Services.Interfaces
{
    public interface IMidiPlayer
    {
        event Action<int, int> PlayingProgress;
        event Action<bool> PlayingState;

        void Pause();
        void PlaySheet(SheetInformation sheetInfo, int tempo);
        void RewindToZero();
        void SetTempo(int tempo);
    }
}