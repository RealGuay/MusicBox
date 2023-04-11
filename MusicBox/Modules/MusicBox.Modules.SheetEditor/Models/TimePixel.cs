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
        private static int nextTimePixelId;
        private static Color leftHandColor = (Color)ColorConverter.ConvertFromString("#FF75EA56");
        private static Color rightHandColor = (Color)ColorConverter.ConvertFromString("#FF408DE4");

        public const int NumberOfWhiteKeys = 52;
        public const int QuarterDuration = 96;   // nb of screen pixels per Quarter note !!!
        public const int ToneHeight = 10;

        private static readonly int DefaultTimeResolution = QuarterDuration / 8;
        public static readonly int ToneResolution = ToneHeight + 2; // leave vertical space between each tone

        public int TimeResolution { get => IsTriplet ? DefaultTimeResolution / 3 : DefaultTimeResolution; }

        private int id;
        private int tone;
        private int position;
        private int duration;
        private NoteAlteration noteAlteration;
        private string noteAlterationSymbol;
        private PlayingHand hand;
        private bool isTriplet;
        private string noteTooltip;

        public int Id { get => id; set => SetProperty(ref id, value); }
        public int Tone { get => tone; set => SetProperty(ref tone, value); }
        public int Position { get => position; set => SetProperty(ref position, value); }
        public int Duration { get => duration; set => SetProperty(ref duration, value, RaiseDurationChanged); }
        public NoteAlteration NoteAlteration { get => noteAlteration; set => SetProperty(ref noteAlteration, value, RefreshNoteAlterationSymbol); }
        public string NoteAlterationSymbol { get => noteAlterationSymbol; set => SetProperty(ref noteAlterationSymbol, value); }
        public PlayingHand Hand { get => hand; set => SetProperty(ref hand, value); }
        public bool IsTriplet { get => isTriplet; set => isTriplet = value; }
        public string NoteTooltip { get => noteTooltip; set => SetProperty(ref noteTooltip, value); }

        public Color HandColor { get => Hand == PlayingHand.Left ? leftHandColor : rightHandColor; }
        public int ToneRectangleHeight { get => ToneHeight; }

        public event EventHandler? DurationChanged;

        private void RaiseDurationChanged()
        {
            DurationChanged?.Invoke(this, EventArgs.Empty);
        }

        private void RefreshNoteAlterationSymbol()
        {
            NoteAlterationSymbol = $"{NoteAlterationSymbols[(int)noteAlteration + 1]}";
        }

        public TimePixel(int tone, int position, PlayingHand hand)
        {
            id = GetNextId();
            this.tone = tone;
            this.position = position;
            this.hand = hand;

            duration = QuarterDuration / 2;
            noteAlteration = NoteAlteration.None;
            RefreshNoteAlterationSymbol();
            isTriplet = false;
            noteTooltip = string.Empty;

            //this.noteTooltip = NoteTooltip;
            DurationChanged = null;
        }

        private int GetNextId()
        {
            return Interlocked.Increment(ref nextTimePixelId);
        }

        public TimePixel DeepCopy()
        {
            TimePixel tp = new TimePixel(tone, position, hand); // note : copy's Id is different from the original

            tp.duration = duration;
            tp.noteAlteration = noteAlteration;
            tp.RefreshNoteAlterationSymbol();
            tp.IsTriplet = isTriplet;
            tp.noteTooltip = noteTooltip;

            tp.RaisePropertyChanged(); // refresh all binding properties  ???

            // Do NOT forget to set DurationChanged when required
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