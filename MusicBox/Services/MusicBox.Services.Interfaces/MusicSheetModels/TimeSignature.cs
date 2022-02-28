namespace MusicBox.Services.Interfaces.MusicSheetModels
{
    public class TimeSignature
    {
        public string  Name { get; set; }
        public int TopNumber { get; }
        public int BottomNumber { get; }
        public int BeatsPerBar { get; }
        public int SubbeatsPerBeat { get; }
        public int NotesPerQuarter { get; }  // type of note used to define one beat (see NotesPerBeat)
        public int NotesPerBeat { get; }

        private TimeSignature(string name, int topNumber, int bottomNumber, int beatsPerBar, int subbeatsPerBeat, int notesPerQuarter, int notesPerBeat)
        {
            Name = name;
            TopNumber = topNumber;
            BottomNumber = bottomNumber;
            BeatsPerBar = beatsPerBar;
            SubbeatsPerBeat = subbeatsPerBeat;
            NotesPerQuarter = notesPerQuarter;
            NotesPerBeat = notesPerBeat;
        }

        public static TimeSignature TS_2_4 { get; } = new TimeSignature("2:4", 2, 4, 2, 4, 1, 1);
        public static TimeSignature TS_3_4 { get; } = new TimeSignature("3:4", 3, 4, 3, 4, 1, 1);
        public static TimeSignature TS_4_4 { get; } = new TimeSignature("4:4", 4, 4, 4, 4, 1, 1);
                                                                        
        public static TimeSignature TS_3_8 { get; } = new TimeSignature("3:8", 3, 8, 1, 3, 2, 3);
        public static TimeSignature TS_6_8 { get; } = new TimeSignature("6:8", 6, 8, 2, 3, 2, 3);
        public static TimeSignature TS_9_8 { get; } = new TimeSignature("9:8", 9, 8, 3, 3, 2, 3);
        public static TimeSignature TS_12_8 { get; } = new TimeSignature("12:8", 12, 8, 4, 3, 2, 3);
    }
}