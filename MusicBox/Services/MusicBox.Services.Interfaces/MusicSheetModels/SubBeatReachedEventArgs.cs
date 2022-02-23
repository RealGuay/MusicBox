using System;

namespace MusicBox.Services.Interfaces.MusicSheetModels
{
    public class SubBeatReachedEventArgs : EventArgs
    {
        public int SubBeatCount;
    }
}