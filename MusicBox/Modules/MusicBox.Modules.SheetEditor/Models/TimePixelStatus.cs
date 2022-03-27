using System;
using System.Collections.Generic;
using System.Text;

namespace MusicBox.Modules.SheetEditor.Models
{
    public enum TimePixelStatus
    {
        OnBlankLine,
        OnStaffLine,
        PixelOn,
        PixelOnAndFlat,
        PixelOnAndSharp
    }
}
