using MusicBox.Services.Interfaces.MusicSheetModels;
using Prism.Mvvm;
using System;

using System.Threading;
using System.Windows;
using System.Windows.Media;
using static MusicBox.Services.Interfaces.MusicSheetModels.ScaleInformation;

namespace MusicBox.Modules.SheetEditor.Models
{
    public class TimePixel : BindableBase
    {
        private static int _nextTimePixelId;
        private static Color leftHandColor = (Color)ColorConverter.ConvertFromString("#FF75EA56");
        private static Color rightHandColor = (Color)ColorConverter.ConvertFromString("#FF408DE4");

        public const int NumberOfWhiteKeys = 52;
        public const int QuarterDuration = 96;   // nb of screen pixels per Quarter note !!!
        public const int ToneHeight = 10;

        private static readonly int DefaultTimeResolution = QuarterDuration / 8;
        public static readonly int ToneResolution = ToneHeight + 2; // leave vertical space between each tone

        public int TimeResolution { get => IsTriplet ? DefaultTimeResolution / 3 : DefaultTimeResolution; }

        private int id;
        private int position;
        private int tone;
        private int duration;

        private NoteAlteration noteAlteration;
        private string noteAlterationSymbol;

        public int Id { get => id; set => SetProperty(ref id, value); }
        public int Tone { get => tone; set => SetProperty(ref tone, value); }
        public int Position { get => position; set => SetProperty(ref position, value); }
        public int Duration { get => duration; set => SetProperty(ref duration, value, OnDurationChanged); }
        public NoteAlteration NoteAlteration { get => noteAlteration; set => SetProperty(ref noteAlteration, value, OnNoteAlterationChanged); }
        public bool IsTriplet { get => isTriplet; set => isTriplet = value; }

        private void OnNoteAlterationChanged()
        {
            NoteAlterationSymbol = $"{NoteAlterationSymbols[(int)noteAlteration + 1]}";
        }

        public string NoteAlterationSymbol { get => noteAlterationSymbol; set => SetProperty(ref noteAlterationSymbol, value); }

        private string noteTooltip;
        public string NoteTooltip { get => noteTooltip; set => SetProperty(ref noteTooltip, value); }

        private PlayingHand hand;
        private bool isTriplet;

        public PlayingHand Hand { get => hand; set => SetProperty(ref hand, value); }

        public Color HandColor { get => Hand == PlayingHand.Left ? leftHandColor : rightHandColor; }

        public int ToneRectangleHeight { get => ToneHeight; }

        public event EventHandler? DurationChanged;

        private void OnDurationChanged()
        {
            DurationChanged?.Invoke(this, EventArgs.Empty);
        }

        public TimePixel(int tone, int position, PlayingHand hand)
        {
            this.id = Interlocked.Increment(ref _nextTimePixelId);
            this.tone = tone;
            this.position = position;
            this.duration = QuarterDuration / 2;
            this.noteAlteration = NoteAlteration.None;
            this.hand = hand;
            isTriplet = false;

            //this.noteTooltip = NoteTooltip;
            DurationChanged = null;
        }

        public TimePixel DeepCopy()
        {
            TimePixel tp = (TimePixel)MemberwiseClone();
            // add reference type copy here...
            return tp;
        }

        public void RotateNoteAlteration()
        {
            NoteAlteration = ScaleInformation.GetNextNoteAlteration(NoteAlteration);
        }

        internal void ModifyDuration(bool increase)
        {
            Duration += increase ? TimeResolution : -TimeResolution;
            Duration = Math.Max(Duration, TimeResolution);
        }

        public static void ConvertMousePositionToToneAndPosition(Point pos, out int position, out int tone)
        {
            position = RoundPosition(pos.X, DefaultTimeResolution);
            tone = RoundPosition(pos.Y, ToneResolution);
        }

        private static int RoundPosition(double value, int round)
        {
            int rounded = (int)(Math.Round(value) / round);
            return rounded * round;
        }

        public void MoveTimePixel(Point pt)
        {
            Position = Math.Max(RoundPosition(pt.X, TimeResolution), 0);
            Tone = Math.Max(RoundPosition(pt.Y, ToneResolution), 0);
        }
    }
}