using System;
using System.Collections.Generic;
using System.Text;

namespace MusicBox.Services.MidiInterfaces
{
    public class MidiNoteToPlay
    {
        public int TickTimeToPlay { get; set; }
        public int MidiKey { get; set; }
        public int Volume { get; set; }
    }
}
