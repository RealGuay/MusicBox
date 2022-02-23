using System;

namespace MusicBox.Services.Interfaces.MusicSheetModels
{
    public class BeatReachedEventArgs : EventArgs
    {
        public int BeatCount;
    }
}