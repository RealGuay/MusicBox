using MusicBox.Services.Interfaces.MusicSheetModels;
using Prism.Mvvm;
using System;
using System.Threading;
using static MusicBox.Services.Interfaces.MusicSheetModels.ScaleInformation;

namespace MusicBox.Modules.SheetEditor.Models
{
    public class TimePixel : BindableBase
    {
        private static int _nextTimePixelId;

        public const int NumberOfWhiteKeys = 52;
        public const int QuarterDuration = 96;   // nb of screen pixels per Quarter note !!!
        public const int ToneHeight = 10;

        private int id;
        private int position;
        private int tone;
        private int duration;
        private string noteAlteration;

        public int Id { get => id; set => SetProperty(ref id, value); }
        public int Position { get => position; set => SetProperty(ref position, value); }
        public int Tone { get => tone; set => SetProperty(ref tone, value); }
        public int Duration { get => duration; set => SetProperty(ref duration, value, OnDurationChanged); }
        public string NoteAlteration { get => noteAlteration; set => SetProperty(ref noteAlteration, value); }






        private string noteTooltip;
        public string NoteTooltip { get => noteTooltip; set => SetProperty(ref noteTooltip, value); }

        //private StaffPart line;
        //public StaffPart Line { get => line; set => SetProperty(ref line, value); }

        //private bool onStaffLine;
        //public bool OnStaffLine { get => onStaffLine; set => SetProperty(ref onStaffLine, value); }


        private PlayingHand hand;
        public PlayingHand Hand { get => hand; set => SetProperty(ref hand, value); }

        //public TimePixel(int duration, StaffPart line, BarAlteration barAlteration, TimePixelStatus status)
        //{
        //    Duration = duration;
        //    Line = line;
        //    NoteKey noteKey = GetKey(line, barAlteration, NoteAlteration.None);
        //    NoteTooltip = noteKey.Name;
        //}





        public int ToneRectangleHeight { get => ToneHeight; }

        public event EventHandler? DurationChanged;

        private void OnDurationChanged()
        {
            DurationChanged?.Invoke(this, EventArgs.Empty);
        }


        public TimePixel(int tone, int position)
        {
            this.id = Interlocked.Increment(ref _nextTimePixelId);
            this.tone = tone;
            this.position = position;
            this.duration = QuarterDuration / 2;
            this.noteAlteration = string.Empty;
            DurationChanged = null;
        }

        public TimePixel DeepCopy()
        {
            TimePixel tp = (TimePixel)MemberwiseClone();
            // add reference type copy here...
            return tp;
        }



    }
}