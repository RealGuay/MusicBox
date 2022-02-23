using System;
using System.Collections.Generic;
using System.Text;

namespace MusicBox.Services.Interfaces.MusicSheetModels
{
    public class BarReachedEventArgs : EventArgs
    {
        public int BarCount;
    }
}
