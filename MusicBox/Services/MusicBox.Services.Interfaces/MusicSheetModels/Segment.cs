using System.Collections.Generic;

namespace MusicBox.Services.Interfaces.MusicSheetModels
{
    public class Segment
    {
        public string Name { get; set; }
        public Context Context { get; set; }
        public int MidiChannel { get; set; }
        public List<Bar> Bars { get; set; }

        public Segment()
        {
            Bars = new List<Bar>();
        }
    }
}