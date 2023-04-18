using System;

namespace MusicBox.Services.Interfaces.MusicSheetModels
{
    public static class ScaleInformation
    {
        public enum NoteAlteration
        {
            Flat = -1,
            None = 0,
            Sharp = 1,
            Natural = 2  // bécarre !!!
        }

        public static char[] NoteAlterationSymbols { get; } = new char[]
        {
            '\u266D',
            ' ',
            '\u266F',
            '\u266E',
        };

        public static NoteAlteration GetNextNoteAlteration(NoteAlteration noteAlteration)
        {
            int current = Array.IndexOf(Enum.GetValues(typeof(NoteAlteration)), noteAlteration);
            int next = ++current % Enum.GetNames(typeof(NoteAlteration)).Length;

            noteAlteration = (NoteAlteration)Enum.GetValues(typeof(NoteAlteration)).GetValue(next);
            return noteAlteration;
        }

        public enum BarAlteration
        {
            Flat5,
            Flat4,
            Flat3,
            Flat2,
            Flat1,
            None,
            Sharp1,
            Sharp2,
            Sharp3,
            Sharp4,
            Sharp5
        }

        private static NoteKey[] DFlatMajor { get; } = new NoteKey[] { NoteKey.C8, NoteKey.B7b, NoteKey.A7b, NoteKey.G7b, NoteKey.F7, NoteKey.E7b, NoteKey.D7b, NoteKey.C7, NoteKey.B6b, NoteKey.A6b, NoteKey.G6b, NoteKey.F6, NoteKey.E6b, NoteKey.D6b, NoteKey.C6, NoteKey.B5b, NoteKey.A5b, NoteKey.G5b, NoteKey.F5, NoteKey.E5b, NoteKey.D5b, NoteKey.C5, NoteKey.B4b, NoteKey.A4b, NoteKey.G4b, NoteKey.F4, NoteKey.E4b, NoteKey.D4b, NoteKey.C4, NoteKey.B3b, NoteKey.A3b, NoteKey.G3b, NoteKey.F3, NoteKey.E3b, NoteKey.D3b, NoteKey.C3, NoteKey.B2b, NoteKey.A2b, NoteKey.G2b, NoteKey.F2, NoteKey.E2b, NoteKey.D2b, NoteKey.C2, NoteKey.B1b, NoteKey.A1b, NoteKey.G1b, NoteKey.F1, NoteKey.E1b, NoteKey.D1b, NoteKey.C1, NoteKey.B0b, NoteKey.A0b };
        private static NoteKey[] AFlatMajor { get; } = new NoteKey[] { NoteKey.C8, NoteKey.B7b, NoteKey.A7b, NoteKey.G7, NoteKey.F7, NoteKey.E7b, NoteKey.D7b, NoteKey.C7, NoteKey.B6b, NoteKey.A6b, NoteKey.G6, NoteKey.F6, NoteKey.E6b, NoteKey.D6b, NoteKey.C6, NoteKey.B5b, NoteKey.A5b, NoteKey.G5, NoteKey.F5, NoteKey.E5b, NoteKey.D5b, NoteKey.C5, NoteKey.B4b, NoteKey.A4b, NoteKey.G4, NoteKey.F4, NoteKey.E4b, NoteKey.D4b, NoteKey.C4, NoteKey.B3b, NoteKey.A3b, NoteKey.G3, NoteKey.F3, NoteKey.E3b, NoteKey.D3b, NoteKey.C3, NoteKey.B2b, NoteKey.A2b, NoteKey.G2, NoteKey.F2, NoteKey.E2b, NoteKey.D2b, NoteKey.C2, NoteKey.B1b, NoteKey.A1b, NoteKey.G1, NoteKey.F1, NoteKey.E1b, NoteKey.D1b, NoteKey.C1, NoteKey.B0b, NoteKey.A0b };
        private static NoteKey[] EFlatMajor { get; } = new NoteKey[] { NoteKey.C8, NoteKey.B7b, NoteKey.A7b, NoteKey.G7, NoteKey.F7, NoteKey.E7b, NoteKey.D7, NoteKey.C7, NoteKey.B6b, NoteKey.A6b, NoteKey.G6, NoteKey.F6, NoteKey.E6b, NoteKey.D6, NoteKey.C6, NoteKey.B5b, NoteKey.A5b, NoteKey.G5, NoteKey.F5, NoteKey.E5b, NoteKey.D5, NoteKey.C5, NoteKey.B4b, NoteKey.A4b, NoteKey.G4, NoteKey.F4, NoteKey.E4b, NoteKey.D4, NoteKey.C4, NoteKey.B3b, NoteKey.A3b, NoteKey.G3, NoteKey.F3, NoteKey.E3b, NoteKey.D3, NoteKey.C3, NoteKey.B2b, NoteKey.A2b, NoteKey.G2, NoteKey.F2, NoteKey.E2b, NoteKey.D2, NoteKey.C2, NoteKey.B1b, NoteKey.A1b, NoteKey.G1, NoteKey.F1, NoteKey.E1b, NoteKey.D1, NoteKey.C1, NoteKey.B0b, NoteKey.A0b };
        private static NoteKey[] BFlatMajor { get; } = new NoteKey[] { NoteKey.C8, NoteKey.B7b, NoteKey.A7, NoteKey.G7, NoteKey.F7, NoteKey.E7b, NoteKey.D7, NoteKey.C7, NoteKey.B6b, NoteKey.A6, NoteKey.G6, NoteKey.F6, NoteKey.E6b, NoteKey.D6, NoteKey.C6, NoteKey.B5b, NoteKey.A5, NoteKey.G5, NoteKey.F5, NoteKey.E5b, NoteKey.D5, NoteKey.C5, NoteKey.B4b, NoteKey.A4, NoteKey.G4, NoteKey.F4, NoteKey.E4b, NoteKey.D4, NoteKey.C4, NoteKey.B3b, NoteKey.A3, NoteKey.G3, NoteKey.F3, NoteKey.E3b, NoteKey.D3, NoteKey.C3, NoteKey.B2b, NoteKey.A2, NoteKey.G2, NoteKey.F2, NoteKey.E2b, NoteKey.D2, NoteKey.C2, NoteKey.B1b, NoteKey.A1, NoteKey.G1, NoteKey.F1, NoteKey.E1b, NoteKey.D1, NoteKey.C1, NoteKey.B0b, NoteKey.A0 };
        private static NoteKey[] FMajor { get; } = new NoteKey[] { NoteKey.C8, NoteKey.B7b, NoteKey.A7, NoteKey.G7, NoteKey.F7, NoteKey.E7, NoteKey.D7, NoteKey.C7, NoteKey.B6b, NoteKey.A6, NoteKey.G6, NoteKey.F6, NoteKey.E6, NoteKey.D6, NoteKey.C6, NoteKey.B5b, NoteKey.A5, NoteKey.G5, NoteKey.F5, NoteKey.E5, NoteKey.D5, NoteKey.C5, NoteKey.B4b, NoteKey.A4, NoteKey.G4, NoteKey.F4, NoteKey.E4, NoteKey.D4, NoteKey.C4, NoteKey.B3b, NoteKey.A3, NoteKey.G3, NoteKey.F3, NoteKey.E3, NoteKey.D3, NoteKey.C3, NoteKey.B2b, NoteKey.A2, NoteKey.G2, NoteKey.F2, NoteKey.E2, NoteKey.D2, NoteKey.C2, NoteKey.B1b, NoteKey.A1, NoteKey.G1, NoteKey.F1, NoteKey.E1, NoteKey.D1, NoteKey.C1, NoteKey.B0b, NoteKey.A0 };

        private static NoteKey[] CMajor { get; } = new NoteKey[] { NoteKey.C8, NoteKey.B7, NoteKey.A7, NoteKey.G7, NoteKey.F7, NoteKey.E7, NoteKey.D7, NoteKey.C7, NoteKey.B6, NoteKey.A6, NoteKey.G6, NoteKey.F6, NoteKey.E6, NoteKey.D6, NoteKey.C6, NoteKey.B5, NoteKey.A5, NoteKey.G5, NoteKey.F5, NoteKey.E5, NoteKey.D5, NoteKey.C5, NoteKey.B4, NoteKey.A4, NoteKey.G4, NoteKey.F4, NoteKey.E4, NoteKey.D4, NoteKey.C4, NoteKey.B3, NoteKey.A3, NoteKey.G3, NoteKey.F3, NoteKey.E3, NoteKey.D3, NoteKey.C3, NoteKey.B2, NoteKey.A2, NoteKey.G2, NoteKey.F2, NoteKey.E2, NoteKey.D2, NoteKey.C2, NoteKey.B1, NoteKey.A1, NoteKey.G1, NoteKey.F1, NoteKey.E1, NoteKey.D1, NoteKey.C1, NoteKey.B0, NoteKey.A0 };

        private static NoteKey[] GMajor { get; } = new NoteKey[] { NoteKey.C8, NoteKey.B7, NoteKey.A7, NoteKey.G7, NoteKey.F7s, NoteKey.E7, NoteKey.D7, NoteKey.C7, NoteKey.B6, NoteKey.A6, NoteKey.G6, NoteKey.F6s, NoteKey.E6, NoteKey.D6, NoteKey.C6, NoteKey.B5, NoteKey.A5, NoteKey.G5, NoteKey.F5s, NoteKey.E5, NoteKey.D5, NoteKey.C5, NoteKey.B4, NoteKey.A4, NoteKey.G4, NoteKey.F4s, NoteKey.E4, NoteKey.D4, NoteKey.C4, NoteKey.B3, NoteKey.A3, NoteKey.G3, NoteKey.F3s, NoteKey.E3, NoteKey.D3, NoteKey.C3, NoteKey.B2, NoteKey.A2, NoteKey.G2, NoteKey.F2s, NoteKey.E2, NoteKey.D2, NoteKey.C2, NoteKey.B1, NoteKey.A1, NoteKey.G1, NoteKey.F1s, NoteKey.E1, NoteKey.D1, NoteKey.C1, NoteKey.B0, NoteKey.A0 };
        private static NoteKey[] DMajor { get; } = new NoteKey[] { NoteKey.C8s, NoteKey.B7, NoteKey.A7, NoteKey.G7, NoteKey.F7s, NoteKey.E7, NoteKey.D7, NoteKey.C7s, NoteKey.B6, NoteKey.A6, NoteKey.G6, NoteKey.F6s, NoteKey.E6, NoteKey.D6, NoteKey.C6s, NoteKey.B5, NoteKey.A5, NoteKey.G5, NoteKey.F5s, NoteKey.E5, NoteKey.D5, NoteKey.C5s, NoteKey.B4, NoteKey.A4, NoteKey.G4, NoteKey.F4s, NoteKey.E4, NoteKey.D4, NoteKey.C4s, NoteKey.B3, NoteKey.A3, NoteKey.G3, NoteKey.F3s, NoteKey.E3, NoteKey.D3, NoteKey.C3s, NoteKey.B2, NoteKey.A2, NoteKey.G2, NoteKey.F2s, NoteKey.E2, NoteKey.D2, NoteKey.C2s, NoteKey.B1, NoteKey.A1, NoteKey.G1, NoteKey.F1s, NoteKey.E1, NoteKey.D1, NoteKey.C1s, NoteKey.B0, NoteKey.A0 };
        private static NoteKey[] AMajor { get; } = new NoteKey[] { NoteKey.C8s, NoteKey.B7, NoteKey.A7, NoteKey.G7s, NoteKey.F7s, NoteKey.E7, NoteKey.D7, NoteKey.C7s, NoteKey.B6, NoteKey.A6, NoteKey.G6s, NoteKey.F6s, NoteKey.E6, NoteKey.D6, NoteKey.C6s, NoteKey.B5, NoteKey.A5, NoteKey.G5s, NoteKey.F5s, NoteKey.E5, NoteKey.D5, NoteKey.C5s, NoteKey.B4, NoteKey.A4, NoteKey.G4s, NoteKey.F4s, NoteKey.E4, NoteKey.D4, NoteKey.C4s, NoteKey.B3, NoteKey.A3, NoteKey.G3s, NoteKey.F3s, NoteKey.E3, NoteKey.D3, NoteKey.C3s, NoteKey.B2, NoteKey.A2, NoteKey.G2s, NoteKey.F2s, NoteKey.E2, NoteKey.D2, NoteKey.C2s, NoteKey.B1, NoteKey.A1, NoteKey.G1s, NoteKey.F1s, NoteKey.E1, NoteKey.D1, NoteKey.C1s, NoteKey.B0, NoteKey.A0 };
        private static NoteKey[] EMajor { get; } = new NoteKey[] { NoteKey.C8s, NoteKey.B7, NoteKey.A7, NoteKey.G7s, NoteKey.F7s, NoteKey.E7, NoteKey.D7s, NoteKey.C7s, NoteKey.B6, NoteKey.A6, NoteKey.G6s, NoteKey.F6s, NoteKey.E6, NoteKey.D6s, NoteKey.C6s, NoteKey.B5, NoteKey.A5, NoteKey.G5s, NoteKey.F5s, NoteKey.E5, NoteKey.D5s, NoteKey.C5s, NoteKey.B4, NoteKey.A4, NoteKey.G4s, NoteKey.F4s, NoteKey.E4, NoteKey.D4s, NoteKey.C4s, NoteKey.B3, NoteKey.A3, NoteKey.G3s, NoteKey.F3s, NoteKey.E3, NoteKey.D3s, NoteKey.C3s, NoteKey.B2, NoteKey.A2, NoteKey.G2s, NoteKey.F2s, NoteKey.E2, NoteKey.D2s, NoteKey.C2s, NoteKey.B1, NoteKey.A1, NoteKey.G1s, NoteKey.F1s, NoteKey.E1, NoteKey.D1s, NoteKey.C1s, NoteKey.B0, NoteKey.A0 };
        private static NoteKey[] BMajor { get; } = new NoteKey[] { NoteKey.C8s, NoteKey.B7, NoteKey.A7s, NoteKey.G7s, NoteKey.F7s, NoteKey.E7, NoteKey.D7s, NoteKey.C7s, NoteKey.B6, NoteKey.A6s, NoteKey.G6s, NoteKey.F6s, NoteKey.E6, NoteKey.D6s, NoteKey.C6s, NoteKey.B5, NoteKey.A5s, NoteKey.G5s, NoteKey.F5s, NoteKey.E5, NoteKey.D5s, NoteKey.C5s, NoteKey.B4, NoteKey.A4s, NoteKey.G4s, NoteKey.F4s, NoteKey.E4, NoteKey.D4s, NoteKey.C4s, NoteKey.B3, NoteKey.A3s, NoteKey.G3s, NoteKey.F3s, NoteKey.E3, NoteKey.D3s, NoteKey.C3s, NoteKey.B2, NoteKey.A2s, NoteKey.G2s, NoteKey.F2s, NoteKey.E2, NoteKey.D2s, NoteKey.C2s, NoteKey.B1, NoteKey.A1s, NoteKey.G1s, NoteKey.F1s, NoteKey.E1, NoteKey.D1s, NoteKey.C1s, NoteKey.B0, NoteKey.A0s };

        public static NoteKey[][] AllScales { get; } = new NoteKey[][] { DFlatMajor, AFlatMajor, EFlatMajor, BFlatMajor, FMajor, CMajor, GMajor, DMajor, AMajor, EMajor, BMajor };

        private static int indexOfCMajor = 5;

        public static NoteKey GetKey(int line, BarAlteration barAlteration, NoteAlteration noteAlteration)
        {
            NoteKey noteKey = AllScales[(int)barAlteration][line];
            return noteKey.AddAlteration(noteAlteration);
        }

        public static void GetTimePixelInfoFromName(string noteName, BarAlteration barAlteration, out int line, out NoteAlteration noteAlteration)
        {
            int scaleIndex = (int)barAlteration;
            noteAlteration = NoteAlteration.None;

            string baseNoteName = noteName[..2];
            int lineIndex = Array.FindIndex(AllScales[scaleIndex], n => n.Name[..2] == baseNoteName);
            if (lineIndex == -1)
            {
                throw new InvalidOperationException($"Unable to find staff line from note name {noteName}!");
            }
            NoteKey NoteOnLine = AllScales[scaleIndex][lineIndex];
            if (NoteOnLine.Name != noteName)
            {
                noteAlteration = SetNoteAlteration(noteName, NoteOnLine.Name);
            }
            line = lineIndex;
        }

        private static NoteAlteration SetNoteAlteration(string noteName, string noteOnLineName)
        {
            if (noteName.Length > noteOnLineName.Length)
            {
                var added = noteName.Substring(noteOnLineName.Length);
                if (added.Length > 0)
                {
                    if (added.StartsWith("b")) return NoteAlteration.Flat; else return NoteAlteration.Sharp;
                }
            }
            else
            {
                var added = noteOnLineName.Substring(noteName.Length);
                if (added.Length > 0)
                {
                    if (added.StartsWith("b")) return NoteAlteration.Sharp; else return NoteAlteration.Flat;
                }
            }
            throw new InvalidOperationException($"Unable to set note alteration: {noteName} {noteOnLineName}");
        }
    }
}