using System;

namespace MusicBox.Services.Interfaces.MusicSheetModels
{
    public static class ScaleInformation
    {
        public enum NoteAlteration
        {
            Flat = -1,
            None = 0,
            Sharp = 1
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

        public static NoteKey GetKey(StaffPart line, BarAlteration barAlteration, NoteAlteration noteAlteration)
        {
            NoteKey [] notes = AllScales[(int)barAlteration];
            NoteKey noteKey = AllScales[(int)barAlteration][(int)line];
            return  noteKey.AddAlteration(noteAlteration);
        }

        public static void GetTimePixelInfoFromKey(string noteName, BarAlteration barAlteration, out StaffPart line, out NoteAlteration noteAlteration)
        {
            noteAlteration = NoteAlteration.None;
            NoteKey noteKey = NoteKey.FindFromName(noteName);

            int lineIndex = Array.IndexOf(AllScales[(int)barAlteration], noteKey);
            if (lineIndex == -1)
            {
                noteAlteration = barAlteration < BarAlteration.None ? NoteAlteration.Flat : NoteAlteration.Sharp;
                NoteKey alteredNoteKey = noteKey.AddAlteration(noteAlteration);
                lineIndex = Array.IndexOf(AllScales[(int)barAlteration], alteredNoteKey);
                if (lineIndex == -1)
                {
                    throw new InvalidOperationException("Unable to find staff line from note key!");
                }
            }
            line = (StaffPart)lineIndex;
        }

        public enum ScaleSelector
        {
            CMajorScale = 0,
        };

        public enum NoteKeyNoFlat
        {
            Cm1 = 0,
            Cm1s = 1,
            Dm1 = 2,
            Dm1s = 3,
            Em1 = 4,
            Fm1 = 5,
            Fm1s = 6,
            Gm1 = 7,
            Gm1s = 8,
            Am1 = 9,
            Am1s = 10,
            Bm1 = 11,

            C0 = 12,
            C0s = 13,
            D0 = 14,
            D0s = 15,
            E0 = 16,
            F0 = 17,
            F0s = 18,
            G0 = 19,
            G0s = 20,
            A0 = 21,  // piano lowest note
            A0s = 22,
            B0 = 23,

            C1 = 24,
            C1s = 25,
            D1 = 26,
            D1s = 27,
            E1 = 28,
            F1 = 29,
            F1s = 30,
            G1 = 31,
            G1s = 32,
            A1 = 33,
            A1s = 34,
            B1 = 35,

            C2 = 36,
            C2s = 37,
            D2 = 38,
            D2s = 39,
            E2 = 40,
            F2 = 41,
            F2s = 42,
            G2 = 43,
            G2s = 44,
            A2 = 45,
            A2s = 46,
            B2 = 47,

            C3 = 48,
            C3s = 49,
            D3 = 50,
            D3s = 51,
            E3 = 52,
            F3 = 53,
            F3s = 54,
            G3 = 55,
            G3s = 56,
            A3 = 57,
            A3s = 58,
            B3 = 59,

            C4 = 60,     // middle C
            C4s = 61,
            D4 = 62,
            D4s = 63,
            E4 = 64,
            F4 = 65,
            F4s = 66,
            G4 = 67,
            G4s = 68,
            A4 = 69,    // ref A : 440 Hz
            A4s = 70,
            B4 = 71,

            C5 = 72,
            C5s = 73,
            D5 = 74,
            D5s = 75,
            E5 = 76,
            F5 = 77,
            F5s = 78,
            G5 = 79,
            G5s = 80,
            A5 = 81,
            A5s = 82,
            B5 = 83,

            C6 = 84,
            C6s = 85,
            D6 = 86,
            D6s = 87,
            E6 = 88,
            F6 = 89,
            F6s = 90,
            G6 = 91,
            G6s = 92,
            A6 = 93,
            A6s = 94,
            B6 = 95,

            C7 = 96,
            C7s = 97,
            D7 = 98,
            D7s = 99,
            E7 = 100,
            F7 = 101,
            F7s = 102,
            G7 = 103,
            G7s = 104,
            A7 = 105,
            A7s = 106,
            B7 = 107,

            C8 = 108, // piano highest note
            C8s = 109,
            D8s = 111,
            E8 = 112,
            F8 = 113,
            F8s = 114,
            G8 = 115,
            G8s = 116,
            A8 = 117,
            A8s = 118,
            B8 = 119,

            C9 = 120,
            C9s = 121,
            D9 = 122,
            D9s = 123,
            E9 = 124,
            F9 = 125,
            F9s = 126,
            G9 = 127,
        }

        public enum NoteKeyNoSharp
        {
            Cm1 = 0,
            Dm1b = 1,
            Dm1 = 2,
            Em1b = 3,
            Em1 = 4,
            Fm1 = 5,
            Gm1b = 6,
            Gm1 = 7,
            Am1b = 8,
            Am1 = 9,
            Bm1b = 10,
            Bm1 = 11,

            C0 = 12,
            D0b = 13,
            D0 = 14,
            E0b = 15,
            E0 = 16,
            F0 = 17,
            G0b = 18,
            G0 = 19,
            A0b = 20,
            A0 = 21,  // piano lowest note
            B0b = 22,
            B0 = 23,

            C1 = 24,
            D1b = 25,
            D1 = 26,
            E1b = 27,
            E1 = 28,
            F1 = 29,
            G1b = 30,
            G1 = 31,
            A1b = 32,
            A1 = 33,
            B1b = 34,
            B1 = 35,

            C2 = 36,
            D2b = 37,
            D2 = 38,
            E2b = 39,
            E2 = 40,
            F2 = 41,
            G2b = 42,
            G2 = 43,
            A2b = 44,
            A2 = 45,
            B2b = 46,
            B2 = 47,

            C3 = 48,
            D3b = 49,
            D3 = 50,
            E3b = 51,
            E3 = 52,
            F3 = 53,
            G3b = 54,
            G3 = 55,
            A3b = 56,
            A3 = 57,
            B3b = 58,
            B3 = 59,

            C4 = 60,     // middle C
            D4b = 61,
            D4 = 62,
            E4b = 63,
            E4 = 64,
            F4 = 65,
            G4b = 66,
            G4 = 67,
            A4b = 68,
            A4 = 69,    // ref A : 440 Hz
            B4b = 70,
            B4 = 71,

            C5 = 72,
            D5b = 73,
            D5 = 74,
            E5b = 75,
            E5 = 76,
            F5 = 77,
            G5b = 78,
            G5 = 79,
            A5b = 80,
            A5 = 81,
            B5b = 82,
            B5 = 83,

            C6 = 84,
            D6b = 85,
            D6 = 86,
            E6b = 87,
            E6 = 88,
            F6 = 89,
            G6b = 90,
            G6 = 91,
            A6b = 92,
            A6 = 93,
            B6b = 94,
            B6 = 95,

            C7 = 96,
            D7b = 97,
            D7 = 98,
            E7b = 99,
            E7 = 100,
            F7 = 101,
            G7b = 102,
            G7 = 103,
            A7b = 104,
            A7 = 105,
            B7b = 106,
            B7 = 107,

            C8 = 108, // piano highest note
            D8b = 109,
            D8 = 110,
            E8b = 111,
            E8 = 112,
            F8 = 113,
            G8b = 114,
            G8 = 115,
            A8b = 116,
            A8 = 117,
            B8b = 118,
            B8 = 119,

            C9 = 120,
            D9b = 121,
            D9 = 122,
            E9b = 123,
            E9 = 124,
            F9 = 125,
            G9b = 126,
            G9 = 127,
        }
    }
}