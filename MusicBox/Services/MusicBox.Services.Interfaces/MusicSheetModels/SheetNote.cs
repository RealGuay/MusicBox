using System;
using System.Collections.Generic;
using System.Text;

namespace MusicBox.Services.Interfaces.MusicSheetModels
{
    public class SheetNote
    {
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

        //internal string TraceDetails()
        //{
        //    string noteDetails = $"Key : {Key}, PositionInBar : {PositionInBar}, Duration : {Duration}, TiedTo : {TiedTo} \n";
        //    return noteDetails;
        //}
    }
}
