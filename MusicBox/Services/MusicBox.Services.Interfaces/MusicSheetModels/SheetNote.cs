using System;
using System.Collections.Generic;
using System.Text;

namespace MusicBox.Services.Interfaces.MusicSheetModels
{
    public class SheetNote
    {
        public string Name { get; set; }
        public int Key { get; set; }
        public int PositionInBar { get; set; }
        public int Duration { get; set; }
        public int Volume { get; set; }
        public Tie TiedTo { get; set; }

        public enum Tie
        {
            ToNext,
            ToPrevious,
            ToBoth,
            ToNone
        }
    }
}
