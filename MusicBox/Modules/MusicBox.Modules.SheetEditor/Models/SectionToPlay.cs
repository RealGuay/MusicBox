using System;
using System.Collections.Generic;
using System.Text;

namespace MusicBox.Modules.SheetEditor.Models
{
    public class SectionToPlay
    {
        public Section Section { get; set; }
        public string Name { get; set; }
    }
    public enum Section
    {
        SHEET,
        SEGMENT,
        BAR
    }
}
