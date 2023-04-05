using MusicBox.Modules.SheetEditor.Models;
using MusicBox.Services.Interfaces.MusicSheetModels;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using static MusicBox.Services.Interfaces.MusicSheetModels.ScaleInformation;

namespace MusicBox.Modules.SheetEditor.ViewModels
{
    public class BarEditorViewModel : BindableBase, IBarEditorViewModel
    {
        private readonly int _positionResolution = TimePixel.QuarterDuration / 8;
        private readonly int _screeenPixelPerTick = TimePixel.QuarterDuration / (int)TickResolution.Normal;

        private readonly int _toneResolution;

        private TimePixel? _storedTimePixel;
        private int barWidth;
        private int barHeight;

        public int BarWidth { get => barWidth; set => SetProperty(ref barWidth, value); }
        public int BarHeight { get => barHeight; set => SetProperty(ref barHeight, value); }

        public ObservableCollection<TimePixel> TimePixels { get; set; }

        private int timePixelPerLine;

        public int TimePixelPerLine
        {
            get { return timePixelPerLine; }
            set { SetProperty(ref timePixelPerLine, value); }
        }

        private PlayingHand hand;
        private TimePixel selectedTimePixel;
        private int selectedTimePixelHeight;
        private int selectedTimePixelWidth;
        private Visibility selectedTimePixelVisibility;

        public PlayingHand Hand { get => hand; set => ChangeSelectedHand(ref hand, value); }

        private void ChangeSelectedHand(ref PlayingHand hand, PlayingHand value, [CallerMemberName] string propertyName = null)
        {
            if (SetProperty(ref hand, value, propertyName))
            {
                RaisePropertyChanged(nameof(IsLeftHandSelected));
                RaisePropertyChanged(nameof(IsRightHandSelected));
            }
        }

        public bool IsLeftHandSelected { get => Hand == PlayingHand.Left; set => SelectHand(PlayingHand.Left, value); }
        public bool IsRightHandSelected { get => Hand == PlayingHand.Right; set => SelectHand(PlayingHand.Right, value); }

        private void SelectHand(PlayingHand hand, bool selected)
        {
            if (selected)
            {
                Hand = hand;
            }
            else
            {
                Hand = hand == PlayingHand.Left ? PlayingHand.Right : PlayingHand.Left;
            }
        }

        public int TimePixelIncrement { get; private set; }

        private readonly IEventAggregator _eventAggregator;
        private readonly TimeSignature _timeSignature;
        private readonly KeySignature _keySignature;

        public DelegateCommand<object> ActivateTimePixel { get; private set; }
        public DelegateCommand<object> SelectTimePixelCommand { get; set; }
        public DelegateCommand<object> DeleteTimePixelCommand { get; private set; }

        public DelegateCommand ChangeSelectedBarCommand { get; set; }

        public TimePixel? SelectedTimePixel { get => selectedTimePixel; set => SetProperty(ref selectedTimePixel, value); }
        public int SelectedTimePixelHeight { get => selectedTimePixelHeight; set => SetProperty(ref selectedTimePixelHeight, value); }
        public int SelectedTimePixelWidth { get => selectedTimePixelWidth; set => SetProperty(ref selectedTimePixelWidth, value); }
        public Visibility SelectedTimePixelVisibility { get => selectedTimePixelVisibility; set => SetProperty(ref selectedTimePixelVisibility, value); }

        public BarEditorViewModel(IEventAggregator eventAggregator, TimeSignature timeSignature, KeySignature keySignature)
        {
            _eventAggregator = eventAggregator;

            ChangeSelectedBarCommand = new DelegateCommand(ChangeSelectedBar);

            _timeSignature = timeSignature;
            _keySignature = keySignature;
            Hand = PlayingHand.Right;

            CalculateBarWidth();
            _toneResolution = TimePixel.ToneHeight + 2; // leave vertical space between each tone
            _storedTimePixel = null;
            BarHeight = TimePixel.NumberOfWhiteKeys * _toneResolution;
            selectedTimePixel = null;
            selectedTimePixelVisibility = Visibility.Hidden;

            TimePixels = new ObservableCollection<TimePixel>();
            ActivateTimePixel = new DelegateCommand<object>(ActivateTimePixelExecute);
            SelectTimePixelCommand = new DelegateCommand<object>(SelectTimePixelExecute);
            DeleteTimePixelCommand = new DelegateCommand<object>(DeleteTimePixelExecute);
        }

        private void SelectTimePixelExecute(object obj)
        {
            var tp = obj as TimePixel;
            if (tp != null)
            {
                SelectedTimePixel = tp;
                SetSelectedTimePixelRectangleDimensions(tp);
                SelectedTimePixelVisibility = Visibility.Visible;
            }
        }

        private void CalculateBarWidth()
        {
            if (_timeSignature.BottomNumber == 4)
            {
                BarWidth = _timeSignature.TopNumber * TimePixel.QuarterDuration;
            }
            else if (_timeSignature.BottomNumber == 8)
            {
                BarWidth = _timeSignature.TopNumber * TimePixel.QuarterDuration / 2;
            }
            else
            {
                throw new ArgumentException($"Invalid time signature : {_timeSignature.TopNumber} / {_timeSignature.BottomNumber} ");
            }
        }

        public void GetVerticalLinesInfo(int positionRectangeWidth, out int beatLines, out int subBeatLines, out int totalLines, out int lineHorizontalSpacing)
        {
            beatLines = _timeSignature.BottomNumber == 8 ? _timeSignature.TopNumber / 3 : _timeSignature.TopNumber;
            subBeatLines = beatLines * (_timeSignature.BottomNumber == 8 ? 2 : 3);
            totalLines = beatLines + subBeatLines;
            lineHorizontalSpacing = positionRectangeWidth / totalLines;
            totalLines++; // end line on right
        }

        internal void GetHorizontalLinesInfo(int actualHeight, out int yTopHorizontalLine, out int verticalSpacingPerTone)
        {
            yTopHorizontalLine = 18 * _toneResolution + TimePixel.ToneHeight / 2;
            verticalSpacingPerTone = _toneResolution * 2;
        }

        private void ActivateTimePixelExecute(object obj)
        {
            var rec = obj as IInputElement;
            if (rec != null)
            {
                var pos = Mouse.GetPosition(rec);
                int position = RoundPosition(pos.X, _positionResolution);
                int tone = RoundPosition(pos.Y, _toneResolution);
                if (!TimePixels.Any(t => t.Tone == tone && t.Position == position))
                {
                    TimePixel tp = new TimePixel(tone, position, hand);
                    tp.DurationChanged += TimePixel_DurationChanged;
                    TimePixels.Add(tp);
                }
            }
        }

        private int RoundPosition(double value, int round)
        {
            int rounded = (int)(Math.Round(value) / round);
            return rounded * round;
        }

        public void ModifyDuration(int id, bool increase)
        {
            var timePixel = FindTimePixel(id);
            bool isTriplet = timePixel.Duration % 3 > 0;
            if (!isTriplet)
            {
                timePixel.Duration += increase ? _positionResolution : -_positionResolution;
            }
            else
            {
                timePixel.Duration *= 3;
                timePixel.Duration /= increase ? 2 : 4;
            }
            timePixel.Duration = Math.Max(timePixel.Duration, _positionResolution);
        }

        private TimePixel FindTimePixel(int id)
        {
            TimePixel tp = TimePixels.Single(p => p.Id == id);
            return tp;
        }

        private void TimePixel_DurationChanged(object? sender, EventArgs e)
        {
            var tp = sender as TimePixel;
            if (tp != null && SelectedTimePixel != null)
            {
                if (tp.Id == SelectedTimePixel.Id)
                {
                    SetSelectedTimePixelRectangleDimensions(tp);
                }
            }
        }

        private void SetSelectedTimePixelRectangleDimensions(TimePixel tp)
        {
            SelectedTimePixelHeight = tp.ToneRectangleHeight + 4;
            SelectedTimePixelWidth = tp.Duration + 4;
        }

        internal void StoreTimePixel(int id)
        {
            TimePixel tp = FindTimePixel(id);
            _storedTimePixel = tp.DeepCopy();
        }

        internal void RecallTimePixel(int id)
        {
            if (_storedTimePixel == null) return;

            TimePixel tp = FindTimePixel(id);
            int index = TimePixels.IndexOf(tp);
            if (index >= 0)
            {
                var tpRestored = _storedTimePixel.DeepCopy();
                TimePixels[index] = tpRestored;
                _storedTimePixel = null;
                //HideCurrentNoteInfo();
            }
        }

        internal void MoveTimePixel(int id, Point pt)
        {
            var timePixel = FindTimePixel(id);
            timePixel.Position = Math.Max(RoundPosition(pt.X, _positionResolution), 0);
            timePixel.Tone = Math.Max(RoundPosition(pt.Y, _toneResolution), 0);
            //DisplayCurrentNoteInfo(id);
        }

        private void DeleteTimePixelExecute(object obj)
        {
            var tp = obj as TimePixel;
            if (tp != null)
            {
                if (tp == SelectedTimePixel)
                {
                    SelectedTimePixelVisibility = Visibility.Hidden;
                    SelectedTimePixel = null;
                }
                TimePixels.Remove(tp);
            }
        }

        /*****************************************************************************************************************************************/
        /*****************************************************************************************************************************************/
        /*****************************************************************************************************************************************/
        /*****************************************************************************************************************************************/

        private void ChangeSelectedBar()
        {
            _eventAggregator.GetEvent<SelectedBarChanged>().Publish(this);
        }

        public IBarEditorViewModel DeepCopy()
        {
            BarEditorViewModel newModel = new BarEditorViewModel(_eventAggregator, _timeSignature, _keySignature);
            return newModel;
        }

        public void ExtractBarInfo(Bar currentBar)
        {
            if (currentBar is null)
            {
                throw new ArgumentNullException(nameof(currentBar));
            }
            ExtractBarNotes(currentBar);
        }

        private void ExtractBarNotes(Bar currentBar)
        {
            SheetNote sheetNote = null;
            foreach (TimePixel pixel in TimePixels)
            {
                NoteKey noteKey = GetKey(pixel.Tone / _toneResolution, _keySignature.BarAlteration, pixel.NoteAlteration);

                sheetNote = new SheetNote
                {
                    Name = noteKey.Name,
                    Key = noteKey.Key,
                    TiedTo = SheetNote.Tie.ToNone,
                    Volume = 100,
                    PositionInBar = pixel.Position / _screeenPixelPerTick,
                    Hand = pixel.Hand
                };
                sheetNote.Duration = pixel.Duration / _screeenPixelPerTick;
                currentBar.SheetNotes.Add(sheetNote);
            }
        }

        public void LoadBarInfo(Bar bar)
        {
            foreach (var note in bar.SheetNotes)
            {
                SetTimePixelsFromNote(note);
            }
        }

        private void SetTimePixelsFromNote(SheetNote note)
        {
            ScaleInformation.GetTimePixelInfoFromName(note.Name, _keySignature.BarAlteration, out int line, out NoteAlteration noteAlteration);

            TimePixel tp = new TimePixel(line * _toneResolution, note.PositionInBar * _screeenPixelPerTick, note.Hand);
            tp.Duration = note.Duration * _screeenPixelPerTick;
            tp.NoteAlteration = noteAlteration;

            TimePixels.Add(tp);
        }

        internal void RotateNoteAlteration(int id)
        {
            TimePixel tp = FindTimePixel(id);
            tp.RotateNoteAlteration();
        }
    }
}