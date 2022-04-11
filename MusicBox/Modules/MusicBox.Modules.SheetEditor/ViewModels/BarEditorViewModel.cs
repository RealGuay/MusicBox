using MusicBox.Modules.SheetEditor.Models;
using MusicBox.Services.Interfaces.MusicSheetModels;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using static MusicBox.Services.Interfaces.MusicSheetModels.ScaleInformation;
using static MusicBox.Services.Interfaces.MusicSheetModels.SheetNote;

namespace MusicBox.Modules.SheetEditor.ViewModels
{
    public class BarEditorViewModel : BindableBase, IBarEditorViewModel
    {
        private BarAlteration barAlteration;

        public BarAlteration BarAlteration
        {
            get { return barAlteration; }
            set { SetProperty(ref barAlteration, value); }
        }

        private List<TimePixel> timePixels;

        public List<TimePixel> TimePixels
        {
            get { return timePixels; }
            set { SetProperty(ref timePixels, value); }
        }

        private TimePixel selectedPixel;

        public TimePixel SelectedPixel
        {
            get { return selectedPixel; }
            set { SetProperty(ref selectedPixel, value); }
        }

        private int timePixelPerLine;

        public int TimePixelPerLine
        {
            get { return timePixelPerLine; }
            set { SetProperty(ref timePixelPerLine, value); }
        }

        public int TimePixelIncrement { get; private set; }

        private readonly IEventAggregator _eventAggregator;
        private readonly TimeSignature _timeSignature;
        private readonly KeySignature _keySignature;

        public DelegateCommand<TimePixel> ActivatePixelCommand { get; set; }
        public DelegateCommand<TimePixel> ExpandPixelCommand { get; set; }
        public DelegateCommand<TimePixel> AlterPixelCommand { get; set; }
        public DelegateCommand ChangeSelectedBarCommand { get; set; }

        public BarEditorViewModel(IEventAggregator eventAggregator, TimeSignature timeSignature, KeySignature keySignature)
        {
            _eventAggregator = eventAggregator;
            ActivatePixelCommand = new DelegateCommand<TimePixel>(ActivatePixel);
            ExpandPixelCommand = new DelegateCommand<TimePixel>(ExpandPixel);
            AlterPixelCommand = new DelegateCommand<TimePixel>(AlterPixel);
            ChangeSelectedBarCommand = new DelegateCommand(ChangeSelectedBar);
            SelectedPixel = new TimePixel(0, StaffPart.GLine2, BarAlteration) { Line = StaffPart.GLine2 };  /// ???????????????
            TimePixels = new List<TimePixel>();
            BarAlteration = BarAlteration.None;
            _timeSignature = timeSignature;
            _keySignature = keySignature;
            CreateSheetStaff();
        }

        private void ChangeSelectedBar()
        {
            _eventAggregator.GetEvent<SelectedBarChanged>().Publish(this);
        }

        public IBarEditorViewModel DeepCopy()
        {
            BarEditorViewModel newModel = new BarEditorViewModel(_eventAggregator, _timeSignature, _keySignature);
            //            newModel.TimeSignature = TimeSignature;
            return newModel;
        }

        internal void ExtractBarInfo(Bar currentBar)
        {
            if (currentBar is null)
            {
                throw new ArgumentNullException(nameof(currentBar));
            }

            IEnumerable<TimePixel> pixelsOn = TimePixels.Where(p => p.Status >= TimePixelStatus.PixelOn);

            bool isExpandedToNextPixel = false;
            SheetNote sheetNote = null;
            foreach (TimePixel pixel in pixelsOn)
            {
                if (!isExpandedToNextPixel)
                {
                    sheetNote = new SheetNote
                    {
                        Key = (int)GetKey(pixel.Line, BarAlteration, ToNoteAlteration(pixel.Status)),
                        TiedTo = Tie.ToNone,
                        Volume = 100,
                        PositionInBar = pixel.StartPosition
                    };
                    sheetNote.Duration += pixel.Duration;
                }
                else
                {
                    sheetNote.Duration += pixel.Duration;
                }
                isExpandedToNextPixel = pixel.IsExpandedToNextPixel;
                if (!isExpandedToNextPixel)
                {
                    currentBar.SheetNotes.Add(sheetNote);
                }
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
            StaffPart line;
            NoteAlteration noteAlteration;
            ScaleInformation.GetTimePixelInfoFromKey(note.Key, BarAlteration, out line, out noteAlteration);
            TimePixel timePixel = TimePixels.Find(p => p.Line == line && p.StartPosition == note.PositionInBar);
            if (timePixel == null)
            {
                throw new InvalidOperationException("Cannot find TimePixel from note!!!");
            }
            while (timePixel != null && note.Duration > 0)
            {
                SetTimePixelStatusFromNoteAlteration(timePixel, noteAlteration);
                note.Duration -= TimePixelIncrement;
                timePixel.IsExpandedToNextPixel = note.Duration > 0;
                timePixel = timePixel.NextPixelInBar;
            }
        }

        private void SetTimePixelStatusFromNoteAlteration(TimePixel timePixel, NoteAlteration noteAlteration)
        {
            if (noteAlteration == NoteAlteration.None)
            {
                timePixel.Status = TimePixelStatus.PixelOn;
            }
            else
            {
                timePixel.Status = noteAlteration == NoteAlteration.Flat ? TimePixelStatus.PixelOnAndFlat : TimePixelStatus.PixelOnAndSharp;
            }
        }

        private NoteAlteration ToNoteAlteration(TimePixelStatus status)
        {
            if (status == TimePixelStatus.PixelOnAndFlat) return NoteAlteration.Flat;
            if (status == TimePixelStatus.PixelOnAndSharp) return NoteAlteration.Sharp;
            return NoteAlteration.None;
        }

        private void CreateSheetStaff()
        {
            const int timePixelPerQuarter = 4; // 1 tp par double-croche
            int notesPerBar = _timeSignature.NotesPerBeat * _timeSignature.BeatsPerBar;

            TimePixelPerLine = notesPerBar * timePixelPerQuarter/ _timeSignature.NotesPerQuarter; 
            TimePixelIncrement = notesPerBar * (int) TickResolution.Normal / TimePixelPerLine;

            // blanks line above the staff
            CreateOneBlankLine(StaffPart.GLine14, TimePixelPerLine, TimePixelIncrement);  // top line
            CreateOneBlankLine(StaffPart.GSpace13, TimePixelPerLine, TimePixelIncrement);
            CreateOneBlankLine(StaffPart.GLine13, TimePixelPerLine, TimePixelIncrement);
            CreateOneBlankLine(StaffPart.GSpace12, TimePixelPerLine, TimePixelIncrement);
            CreateOneBlankLine(StaffPart.GLine12, TimePixelPerLine, TimePixelIncrement);
            CreateOneBlankLine(StaffPart.GSpace11, TimePixelPerLine, TimePixelIncrement);
            CreateOneBlankLine(StaffPart.GLine11, TimePixelPerLine, TimePixelIncrement);
            CreateOneBlankLine(StaffPart.GSpace10, TimePixelPerLine, TimePixelIncrement);
            CreateOneBlankLine(StaffPart.GLine10, TimePixelPerLine, TimePixelIncrement);
            CreateOneBlankLine(StaffPart.GSpace9, TimePixelPerLine, TimePixelIncrement);
            CreateOneBlankLine(StaffPart.GLine9, TimePixelPerLine, TimePixelIncrement);
            CreateOneBlankLine(StaffPart.GSpace8, TimePixelPerLine, TimePixelIncrement);
            CreateOneBlankLine(StaffPart.GLine8, TimePixelPerLine, TimePixelIncrement);
            CreateOneBlankLine(StaffPart.GSpace7, TimePixelPerLine, TimePixelIncrement);
            CreateOneBlankLine(StaffPart.GLine7, TimePixelPerLine, TimePixelIncrement);
            CreateOneBlankLine(StaffPart.GSpace6, TimePixelPerLine, TimePixelIncrement);
            CreateOneBlankLine(StaffPart.GLine6, TimePixelPerLine, TimePixelIncrement);
            CreateOneBlankLine(StaffPart.GSpace5, TimePixelPerLine, TimePixelIncrement);
            // the StaffPart.G staff
            CreateOneStaffLine(StaffPart.GLine5, TimePixelPerLine, TimePixelIncrement);
            CreateOneBlankLine(StaffPart.GSpace4, TimePixelPerLine, TimePixelIncrement);
            CreateOneStaffLine(StaffPart.GLine4, TimePixelPerLine, TimePixelIncrement);
            CreateOneBlankLine(StaffPart.GSpace3, TimePixelPerLine, TimePixelIncrement);
            CreateOneStaffLine(StaffPart.GLine3, TimePixelPerLine, TimePixelIncrement);
            CreateOneBlankLine(StaffPart.GSpace2, TimePixelPerLine, TimePixelIncrement);
            CreateOneStaffLine(StaffPart.GLine2, TimePixelPerLine, TimePixelIncrement);
            CreateOneBlankLine(StaffPart.GSpace1, TimePixelPerLine, TimePixelIncrement);
            CreateOneStaffLine(StaffPart.GLine1, TimePixelPerLine, TimePixelIncrement);
            // blanks line below the StaffPart.G staff
            CreateOneBlankLine(StaffPart.GSpace0, TimePixelPerLine, TimePixelIncrement);
            CreateOneBlankLine(StaffPart.GLine0, TimePixelPerLine, TimePixelIncrement);
            CreateOneBlankLine(StaffPart.FSpace5, TimePixelPerLine, TimePixelIncrement);
            // the F staff
            CreateOneStaffLine(StaffPart.FLine5, TimePixelPerLine, TimePixelIncrement);
            CreateOneBlankLine(StaffPart.FSpace4, TimePixelPerLine, TimePixelIncrement);
            CreateOneStaffLine(StaffPart.FLine4, TimePixelPerLine, TimePixelIncrement);
            CreateOneBlankLine(StaffPart.FSpace3, TimePixelPerLine, TimePixelIncrement);
            CreateOneStaffLine(StaffPart.FLine3, TimePixelPerLine, TimePixelIncrement);
            CreateOneBlankLine(StaffPart.FSpace2, TimePixelPerLine, TimePixelIncrement);
            CreateOneStaffLine(StaffPart.FLine2, TimePixelPerLine, TimePixelIncrement);
            CreateOneBlankLine(StaffPart.FSpace1, TimePixelPerLine, TimePixelIncrement);
            CreateOneStaffLine(StaffPart.FLine1, TimePixelPerLine, TimePixelIncrement);
            // blanks line below the F staff
            CreateOneBlankLine(StaffPart.FSpace0, TimePixelPerLine, TimePixelIncrement);
            CreateOneBlankLine(StaffPart.FLine0, TimePixelPerLine, TimePixelIncrement);
            CreateOneBlankLine(StaffPart.FSpaceM1, TimePixelPerLine, TimePixelIncrement);
            CreateOneBlankLine(StaffPart.FLineM1, TimePixelPerLine, TimePixelIncrement);
            CreateOneBlankLine(StaffPart.FSpaceM2, TimePixelPerLine, TimePixelIncrement);
            CreateOneBlankLine(StaffPart.FLineM2, TimePixelPerLine, TimePixelIncrement);
            CreateOneBlankLine(StaffPart.FSpaceM3, TimePixelPerLine, TimePixelIncrement);
            CreateOneBlankLine(StaffPart.FLineM3, TimePixelPerLine, TimePixelIncrement);
            CreateOneBlankLine(StaffPart.FSpaceM4, TimePixelPerLine, TimePixelIncrement);
            CreateOneBlankLine(StaffPart.FLineM4, TimePixelPerLine, TimePixelIncrement);
            CreateOneBlankLine(StaffPart.FSpaceM5, TimePixelPerLine, TimePixelIncrement);
            CreateOneBlankLine(StaffPart.FLineM5, TimePixelPerLine, TimePixelIncrement);
            CreateOneBlankLine(StaffPart.FSpaceM6, TimePixelPerLine, TimePixelIncrement);
        }

        private void CreateOneStaffLine(StaffPart line, int timePixelPerLine, int timePixelIncrement)
        {
            CreateOneLine(true, line, timePixelPerLine, timePixelIncrement);
        }

        private void CreateOneBlankLine(StaffPart line, int timePixelPerLine, int timePixelIncrement)
        {
            CreateOneLine(false, line, timePixelPerLine, timePixelIncrement);
        }

        private void CreateOneLine(bool isPixelOnStaffLine, StaffPart line, int timePixelPerLine, int timePixelIncrement)
        {
            TimePixel previousPixel = null;
            int startPosition = 0;

            for (int i = 0; i < timePixelPerLine; i++)
            {
                startPosition = i * timePixelIncrement;
                TimePixel pixel = CreateNewPixel(isPixelOnStaffLine, line, startPosition, timePixelIncrement);
                if (previousPixel != null)
                {
                    previousPixel.NextPixelInBar = pixel;
                    pixel.PreviousPixelInBar = previousPixel;
                }
                TimePixels.Add(pixel);
                previousPixel = pixel;
            }
        }

        private TimePixel CreateNewPixel(bool isPixelOnStaffLine, StaffPart line, int startPosition, int timePixelIncrement)
        {
            return isPixelOnStaffLine
                ? new TimePixel(timePixelIncrement, line, BarAlteration) { OnStaffLine = true, StartPosition = startPosition, Status = TimePixelStatus.OnStaffLine, IsExpandedToNextPixel = false }
                : new TimePixel(timePixelIncrement, line, BarAlteration) { OnStaffLine = false, StartPosition = startPosition, Status = TimePixelStatus.OnBlankLine, IsExpandedToNextPixel = false };
        }

        private void ActivatePixel(TimePixel timePixel)
        {
            if (timePixel.Status < TimePixelStatus.PixelOn)
            {
                SetPixelBackgroundOn(timePixel, TimePixelStatus.PixelOn);
            }
            else
            {
                SetPixelBackgroundOff(timePixel);
                BreakExpansionWithPrevious(timePixel);
            }
        }

        private void AlterPixel(TimePixel timePixel)
        {
            TimePixel firstTimePixel = FindFirstTimePixel(timePixel);
            if (firstTimePixel.Status >= TimePixelStatus.PixelOn)
            {
                TimePixelStatus nextStatus = RotatePixelOnStatus(firstTimePixel);
                SetTimePixelStatus(firstTimePixel, nextStatus);
                PropagateToNextPixel(firstTimePixel);
            }
        }

        private void SetTimePixelStatus(TimePixel timePixel, TimePixelStatus status)
        {
            timePixel.Status = status;
            NoteKey noteKey = GetKey(timePixel.Line, BarAlteration, ToNoteAlteration(timePixel.Status));
            timePixel.NoteTooltip = Enum.GetName(typeof(NoteKey), noteKey);
        }

        private TimePixel FindFirstTimePixel(TimePixel timePixel)
        {
            if (timePixel.PreviousPixelInBar == null || !timePixel.PreviousPixelInBar.IsExpandedToNextPixel)
            {
                return timePixel;
            }
            return FindFirstTimePixel(timePixel.PreviousPixelInBar);
        }

        private void PropagateToNextPixel(TimePixel timePixel)
        {
            if (timePixel == null)
            {
                return;
            }
            if (timePixel.IsExpandedToNextPixel && timePixel.NextPixelInBar != null)
            {
                SetPixelBackgroundOn(timePixel.NextPixelInBar, timePixel.Status);
                PropagateToNextPixel(timePixel.NextPixelInBar);
            }
        }

        private static TimePixelStatus RotatePixelOnStatus(TimePixel timePixel)
        {
            TimePixelStatus nextStatus = (TimePixelStatus)(((int)timePixel.Status + 1) % Enum.GetNames(typeof(TimePixelStatus)).Length);
            if (nextStatus < TimePixelStatus.PixelOn)
            {
                nextStatus = TimePixelStatus.PixelOn;
            }
            return nextStatus;
        }

        private void BreakExpansionWithNeighbours(TimePixel timePixel)
        {
            BreakExpansionWithPrevious(timePixel);
            BreakExpansionWithNext(timePixel);
        }

        private void BreakExpansionWithNext(TimePixel timePixel)
        {
            if (timePixel.IsExpandedToNextPixel)
            {
                if (timePixel.NextPixelInBar != null)
                {
                    BreakExpansionWithNext(timePixel.NextPixelInBar);
                }
                timePixel.IsExpandedToNextPixel = false;
            }
        }

        private void BreakExpansionWithPrevious(TimePixel timePixel)
        {
            TimePixel prev = timePixel.PreviousPixelInBar;
            if (prev != null)
            {
                if (prev.IsExpandedToNextPixel)
                {
                    prev.IsExpandedToNextPixel = false;
                }
            }
        }

        private void SetPixelBackgroundOff(TimePixel timePixel)
        {
            //         timePixel.Status = timePixel.OnStaffLine ? TimePixelStatus.OnStaffLine : TimePixelStatus.OnBlankLine;
            TimePixelStatus status = timePixel.OnStaffLine ? TimePixelStatus.OnStaffLine : TimePixelStatus.OnBlankLine;
            SetTimePixelStatus(timePixel, status);
            timePixel.IsExpandedToNextPixel = false;
        }

        private void SetPixelBackgroundOn(TimePixel timePixel, TimePixelStatus status)
        {
            SetTimePixelStatus(timePixel, status);
            //        timePixel.Status = status;
        }

        private void ExpandPixel(TimePixel timePixel)
        {
            if (timePixel.Status < TimePixelStatus.PixelOn)
            {
                ActivatePixel(timePixel);
            }
            else
            {
                ExpandToNextPixel(timePixel);
            }
        }

        private void ExpandToNextPixel(TimePixel timePixel)
        {
            if (timePixel == null)
            {
                return;
            }

            if (timePixel.IsExpandedToNextPixel)
            {
                ExpandToNextPixel(timePixel.NextPixelInBar);
            }
            else
            {
                SetNextPixelOn(timePixel);
            }
        }

        private void SetNextPixelOn(TimePixel timePixel)
        {
            if (timePixel.NextPixelInBar != null)
            {
                timePixel.IsExpandedToNextPixel = true;
                SetPixelBackgroundOn(timePixel.NextPixelInBar, timePixel.Status);
            }
        }
    }
}