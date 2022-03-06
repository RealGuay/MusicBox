using System;
using System.Collections.Generic;
using System.Text;

namespace MusicBox.Services.Interfaces.MusicSheetModels
{
    public class Context
    {
        public TimeSignature TimeSignature { get; set; }
        public int Tempo { get; set; }
    }
}
