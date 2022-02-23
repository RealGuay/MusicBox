namespace MusicBox.Services.Interfaces.MusicSheetModels
{
    public class TimeSignature
    {
        public int BeatsPerBar { get; }
        public int TypeOfNote { get; }
        public int SubbeatsPerBeat { get; }
        public int NotesPerBeat { get; }

        private TimeSignature(int beatsPerBar, int typeOfNote, int subbeatsPerBeat, int notesPerBeat)
        {
            BeatsPerBar = beatsPerBar;
            TypeOfNote = typeOfNote;
            SubbeatsPerBeat = subbeatsPerBeat;
            NotesPerBeat = notesPerBeat;
        }

        public static TimeSignature CreateTimeSignature4_4()
        {
            return new TimeSignature(4, 4, 4, 1);
        }

        public static TimeSignature CreateTimeSignature12_8()
        {
            return new TimeSignature(4, 8, 3, 3);
        }
    }
}