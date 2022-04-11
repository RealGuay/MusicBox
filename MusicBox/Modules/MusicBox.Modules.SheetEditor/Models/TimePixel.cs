using MusicBox.Services.Interfaces.MusicSheetModels;
using Prism.Mvvm;
using System;
using static MusicBox.Services.Interfaces.MusicSheetModels.ScaleInformation;

namespace MusicBox.Modules.SheetEditor.Models
{
    public class TimePixel : BindableBase
    {
        private int startPosition;
        public int StartPosition { get => startPosition; set => SetProperty(ref startPosition, value); }

        private int duration;
        public int Duration { get => duration; set => SetProperty(ref duration, value); }

        private string noteTooltip;
        public string NoteTooltip { get => noteTooltip; set => SetProperty(ref noteTooltip, value); }

        private StaffPart line;
        public StaffPart Line { get => line; set => SetProperty(ref line, value); }

        private bool onStaffLine;
        public bool OnStaffLine { get => onStaffLine; set => SetProperty(ref onStaffLine, value); }

        private TimePixelStatus status;
        public TimePixelStatus Status { get => status; set => SetProperty(ref status, value); }

        private TimePixel previousPixelInBar;
        public TimePixel PreviousPixelInBar { get => previousPixelInBar; set => SetProperty(ref previousPixelInBar, value); }

        private TimePixel nextPixelInBar;
        public TimePixel NextPixelInBar { get => nextPixelInBar; set => SetProperty(ref nextPixelInBar, value); }

        private bool isExpandedToNextPixel;
        public bool IsExpandedToNextPixel { get => isExpandedToNextPixel; set => SetProperty(ref isExpandedToNextPixel, value); }

        public TimePixel(int duration, StaffPart line, BarAlteration barAlteration)
        {
            Duration = duration;
            Line = line;
            NoteKey noteKey = GetKey(line, barAlteration, NoteAlteration.None);
            NoteTooltip = noteKey.Name;
        }
    }
}