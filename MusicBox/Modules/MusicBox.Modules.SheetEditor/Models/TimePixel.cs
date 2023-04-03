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

        private NoteAlteration noteAlteration;
        private string noteAlterationSymbol;

        public int Id { get => id; set => SetProperty(ref id, value); }
        public int Position { get => position; set => SetProperty(ref position, value); }
        public int Tone { get => tone; set => SetProperty(ref tone, value); }
        public int Duration { get => duration; set => SetProperty(ref duration, value, OnDurationChanged); }
        public NoteAlteration NoteAlteration { get => noteAlteration; set => SetProperty(ref noteAlteration, value, OnNoteAlterationChanged); }

        private void OnNoteAlterationChanged()
        {
            NoteAlterationSymbol = $"{NoteAlterationSymbols[(int)noteAlteration + 1]}";
        }

        public string NoteAlterationSymbol { get => noteAlterationSymbol; set => SetProperty(ref noteAlterationSymbol,value); }

        private string noteTooltip;
        public string NoteTooltip { get => noteTooltip; set => SetProperty(ref noteTooltip, value); }

        private PlayingHand hand;

        public PlayingHand Hand { get => hand; set => SetProperty(ref hand, value); }

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
            this.noteAlteration = NoteAlteration.None;

            //this.noteTooltip = NoteTooltip;
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