using System;
using System.Collections.Generic;
using System.Text;

namespace MusicBox.Services.Interfaces.MusicSheetModels
{
    public class SheetInformation
    {
        public string Title { get; set; }
        public string LyricsBy { get; set; }
        public string MusicBy { get; set; }
        public string Version { get; set; }
        public Context Context { get; set; }

        public SheetInformation(Context context)
        {
            Context = context;
        }
    }
}
