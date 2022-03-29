using System.Collections.Generic;

namespace MusicBox.Services.Interfaces.MusicSheetModels
{
    public class SheetInformation
    {
        public string Title { get; set; }
        public string LyricsBy { get; set; }
        public string MusicBy { get; set; }
        public string Version { get; set; }
        public Context Context { get; set; }

        public List<Segment> Segments { get; set; }

        public SheetInformation(Context context)
        {
            Context = context;
            Segments = new List<Segment>();
        }
    }
}