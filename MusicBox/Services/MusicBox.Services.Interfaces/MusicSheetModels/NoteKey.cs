using System;
using static MusicBox.Services.Interfaces.MusicSheetModels.ScaleInformation;

namespace MusicBox.Services.Interfaces.MusicSheetModels
{
    public class NoteKey
    {
        public string Name { get; set; }
        public int Key { get; set; }

        private NoteKey(string name, int key)
        {
            Name = name;
            Key = key;
        }

        public NoteKey AddAlteration(NoteAlteration noteAlteration)
        {
            int index;

            if (noteAlteration == NoteAlteration.None)
            {
                return this;
            }
            else if (noteAlteration == NoteAlteration.Natural)
            {
                if (Name[^1] == 'b' || Name[^1] == 's')
                {
                    var naturalName = Name.Substring(0, Name.Length - 1);
                    index = Array.FindIndex(NoteKeys, n => n.Name == naturalName);
                    return NoteKeys[index];
                }
                return this;
            }
            else if( noteAlteration == NoteAlteration.Flat)
            {
                if(Name[^1] == 's')
                {
                    string alteredName = Name.Substring(0, Name.Length -1);
                    return new NoteKey(alteredName, Key - 1);
                }
                else
                {
                    string alteredName = Name + "b";
                    return new NoteKey(alteredName, Key - 1);
                }

            }
            else if (noteAlteration == NoteAlteration.Sharp)
            {
                if (Name[^1] == 'b')
                {
                    string alteredName = Name.Substring(0, Name.Length - 1);
                    return new NoteKey(alteredName, Key + 1);
                }
                else
                {
                    string alteredName = Name + "s";
                    return new NoteKey(alteredName, Key + 1);
                }
            }
            throw new InvalidOperationException($"Invalid NoteAlteration: {noteAlteration}");
        }

        public static NoteKey FindFromName(string name)
        {
            return Array.Find(NoteKeys, (n => n.Name == name));
        }

        public static NoteKey Cm1 { get; } = new NoteKey("Cm1", 0);
        public static NoteKey Cm1s { get; } = new NoteKey("Cm1s", 1);
        public static NoteKey Dm1b { get; } = new NoteKey("Dm1b", 1);
        public static NoteKey Dm1 { get; } = new NoteKey("Dm1", 2);
        public static NoteKey Dm1s { get; } = new NoteKey("Dm1s", 3);
        public static NoteKey Em1b { get; } = new NoteKey("Em1b", 3);
        public static NoteKey Em1 { get; } = new NoteKey("Em1", 4);
        public static NoteKey Fm1 { get; } = new NoteKey("Fm1", 5);
        public static NoteKey Fm1s { get; } = new NoteKey("Fm1s", 6);
        public static NoteKey Gm1b { get; } = new NoteKey("Gm1b", 6);
        public static NoteKey Gm1 { get; } = new NoteKey("Gm1", 7);
        public static NoteKey Gm1s { get; } = new NoteKey("Gm1s", 8);
        public static NoteKey Am1b { get; } = new NoteKey("Am1b", 8);
        public static NoteKey Am1 { get; } = new NoteKey("Am1", 9);
        public static NoteKey Am1s { get; } = new NoteKey("Am1s", 10);
        public static NoteKey Bm1b { get; } = new NoteKey("Bm1b", 10);
        public static NoteKey Bm1 { get; } = new NoteKey("Bm1", 11);

        public static NoteKey C0 { get; } = new NoteKey("C0", 12);
        public static NoteKey C0s { get; } = new NoteKey("C0s", 13);
        public static NoteKey D0b { get; } = new NoteKey("D0b", 13);
        public static NoteKey D0 { get; } = new NoteKey("D0", 14);
        public static NoteKey D0s { get; } = new NoteKey("D0s", 15);
        public static NoteKey E0b { get; } = new NoteKey("E0b", 15);
        public static NoteKey E0 { get; } = new NoteKey("E0", 16);
        public static NoteKey F0 { get; } = new NoteKey("F0", 17);
        public static NoteKey F0s { get; } = new NoteKey("F0s", 18);
        public static NoteKey G0b { get; } = new NoteKey("G0b", 18);
        public static NoteKey G0 { get; } = new NoteKey("G0", 19);
        public static NoteKey G0s { get; } = new NoteKey("G0s", 20);
        public static NoteKey A0b { get; } = new NoteKey("A0b", 20);
        public static NoteKey A0 { get; } = new NoteKey("A0", 21);       // piano lowest note
        public static NoteKey A0s { get; } = new NoteKey("A0s", 22);
        public static NoteKey B0b { get; } = new NoteKey("B0b", 22);
        public static NoteKey B0 { get; } = new NoteKey("B0", 23);

        public static NoteKey C1 { get; } = new NoteKey("C1", 24);
        public static NoteKey C1s { get; } = new NoteKey("C1s", 25);
        public static NoteKey D1b { get; } = new NoteKey("D1b", 25);
        public static NoteKey D1 { get; } = new NoteKey("D1", 26);
        public static NoteKey D1s { get; } = new NoteKey("D1s", 27);
        public static NoteKey E1b { get; } = new NoteKey("E1b", 27);
        public static NoteKey E1 { get; } = new NoteKey("E1", 28);
        public static NoteKey F1 { get; } = new NoteKey("F1", 29);
        public static NoteKey F1s { get; } = new NoteKey("F1s", 30);
        public static NoteKey G1b { get; } = new NoteKey("G1b", 30);
        public static NoteKey G1 { get; } = new NoteKey("G1", 31);
        public static NoteKey G1s { get; } = new NoteKey("G1s", 32);
        public static NoteKey A1b { get; } = new NoteKey("A1b", 32);
        public static NoteKey A1 { get; } = new NoteKey("A1", 33);
        public static NoteKey A1s { get; } = new NoteKey("A1s", 34);
        public static NoteKey B1b { get; } = new NoteKey("B1b", 34);
        public static NoteKey B1 { get; } = new NoteKey("B1", 35);

        public static NoteKey C2 { get; } = new NoteKey("C2", 36);
        public static NoteKey C2s { get; } = new NoteKey("C2s", 37);
        public static NoteKey D2b { get; } = new NoteKey("D2b", 37);
        public static NoteKey D2 { get; } = new NoteKey("D2", 38);
        public static NoteKey D2s { get; } = new NoteKey("D2s", 39);
        public static NoteKey E2b { get; } = new NoteKey("E2b", 39);
        public static NoteKey E2 { get; } = new NoteKey("E2", 40);
        public static NoteKey F2 { get; } = new NoteKey("F2", 41);
        public static NoteKey F2s { get; } = new NoteKey("F2s", 42);
        public static NoteKey G2b { get; } = new NoteKey("G2b", 42);
        public static NoteKey G2 { get; } = new NoteKey("G2", 43);
        public static NoteKey G2s { get; } = new NoteKey("G2s", 44);
        public static NoteKey A2b { get; } = new NoteKey("A2b", 44);
        public static NoteKey A2 { get; } = new NoteKey("A2", 45);
        public static NoteKey A2s { get; } = new NoteKey("A2s", 46);
        public static NoteKey B2b { get; } = new NoteKey("B2b", 46);
        public static NoteKey B2 { get; } = new NoteKey("B2", 47);

        public static NoteKey C3 { get; } = new NoteKey("C3", 48);
        public static NoteKey C3s { get; } = new NoteKey("C3s", 49);
        public static NoteKey D3b { get; } = new NoteKey("D3b", 49);
        public static NoteKey D3 { get; } = new NoteKey("D3", 50);
        public static NoteKey D3s { get; } = new NoteKey("D3s", 51);
        public static NoteKey E3b { get; } = new NoteKey("E3b", 51);
        public static NoteKey E3 { get; } = new NoteKey("E3", 52);
        public static NoteKey F3 { get; } = new NoteKey("F3", 53);
        public static NoteKey F3s { get; } = new NoteKey("F3s", 54);
        public static NoteKey G3b { get; } = new NoteKey("G3b", 54);
        public static NoteKey G3 { get; } = new NoteKey("G3", 55);
        public static NoteKey G3s { get; } = new NoteKey("G3s", 56);
        public static NoteKey A3b { get; } = new NoteKey("A3b", 56);
        public static NoteKey A3 { get; } = new NoteKey("A3", 57);
        public static NoteKey A3s { get; } = new NoteKey("A3s", 58);
        public static NoteKey B3b { get; } = new NoteKey("B3b", 58);
        public static NoteKey B3 { get; } = new NoteKey("B3", 59);

        public static NoteKey C4 { get; } = new NoteKey("C4", 60);       // middle C
        public static NoteKey C4s { get; } = new NoteKey("C4s", 61);
        public static NoteKey D4b { get; } = new NoteKey("D4b", 61);
        public static NoteKey D4 { get; } = new NoteKey("D4", 62);
        public static NoteKey D4s { get; } = new NoteKey("D4s", 63);
        public static NoteKey E4b { get; } = new NoteKey("E4b", 63);
        public static NoteKey E4 { get; } = new NoteKey("E4", 64);
        public static NoteKey F4 { get; } = new NoteKey("F4", 65);
        public static NoteKey F4s { get; } = new NoteKey("F4s", 66);
        public static NoteKey G4b { get; } = new NoteKey("G4b", 66);
        public static NoteKey G4 { get; } = new NoteKey("G4", 67);
        public static NoteKey G4s { get; } = new NoteKey("G4s", 68);
        public static NoteKey A4b { get; } = new NoteKey("A4b", 68);
        public static NoteKey A4 { get; } = new NoteKey("A4", 69);       // ref A : 440 Hz
        public static NoteKey A4s { get; } = new NoteKey("A4s", 70);
        public static NoteKey B4b { get; } = new NoteKey("B4b", 70);
        public static NoteKey B4 { get; } = new NoteKey("B4", 71);

        public static NoteKey C5 { get; } = new NoteKey("C5", 72);
        public static NoteKey C5s { get; } = new NoteKey("C5s", 73);
        public static NoteKey D5b { get; } = new NoteKey("D5b", 73);
        public static NoteKey D5 { get; } = new NoteKey("D5", 74);
        public static NoteKey D5s { get; } = new NoteKey("D5s", 75);
        public static NoteKey E5b { get; } = new NoteKey("E5b", 75);
        public static NoteKey E5 { get; } = new NoteKey("E5", 76);
        public static NoteKey F5 { get; } = new NoteKey("F5", 77);
        public static NoteKey F5s { get; } = new NoteKey("F5s", 78);
        public static NoteKey G5b { get; } = new NoteKey("G5b", 78);
        public static NoteKey G5 { get; } = new NoteKey("G5", 79);
        public static NoteKey G5s { get; } = new NoteKey("G5s", 80);
        public static NoteKey A5b { get; } = new NoteKey("A5b", 80);
        public static NoteKey A5 { get; } = new NoteKey("A5", 81);
        public static NoteKey A5s { get; } = new NoteKey("A5s", 82);
        public static NoteKey B5b { get; } = new NoteKey("B5b", 82);
        public static NoteKey B5 { get; } = new NoteKey("B5", 83);

        public static NoteKey C6 { get; } = new NoteKey("C6", 84);
        public static NoteKey C6s { get; } = new NoteKey("C6s", 85);
        public static NoteKey D6b { get; } = new NoteKey("D6b", 85);
        public static NoteKey D6 { get; } = new NoteKey("D6", 86);
        public static NoteKey D6s { get; } = new NoteKey("D6s", 87);
        public static NoteKey E6b { get; } = new NoteKey("E6b", 87);
        public static NoteKey E6 { get; } = new NoteKey("E6", 88);
        public static NoteKey F6 { get; } = new NoteKey("F6", 89);
        public static NoteKey F6s { get; } = new NoteKey("F6s", 90);
        public static NoteKey G6b { get; } = new NoteKey("G6b", 90);
        public static NoteKey G6 { get; } = new NoteKey("G6", 91);
        public static NoteKey G6s { get; } = new NoteKey("G6s", 92);
        public static NoteKey A6b { get; } = new NoteKey("A6b", 92);
        public static NoteKey A6 { get; } = new NoteKey("A6", 93);
        public static NoteKey A6s { get; } = new NoteKey("A6s", 94);
        public static NoteKey B6b { get; } = new NoteKey("B6b", 94);
        public static NoteKey B6 { get; } = new NoteKey("B6", 95);

        public static NoteKey C7 { get; } = new NoteKey("C7", 96);
        public static NoteKey C7s { get; } = new NoteKey("C7s", 97);
        public static NoteKey D7b { get; } = new NoteKey("D7b", 97);
        public static NoteKey D7 { get; } = new NoteKey("D7", 98);
        public static NoteKey D7s { get; } = new NoteKey("D7s", 99);
        public static NoteKey E7b { get; } = new NoteKey("E7b", 99);
        public static NoteKey E7 { get; } = new NoteKey("E7", 100);
        public static NoteKey F7 { get; } = new NoteKey("F7", 101);
        public static NoteKey F7s { get; } = new NoteKey("F7s", 102);
        public static NoteKey G7b { get; } = new NoteKey("G7b", 102);
        public static NoteKey G7 { get; } = new NoteKey("G7", 103);
        public static NoteKey G7s { get; } = new NoteKey("G7s", 104);
        public static NoteKey A7b { get; } = new NoteKey("A7b", 104);
        public static NoteKey A7 { get; } = new NoteKey("A7", 105);
        public static NoteKey A7s { get; } = new NoteKey("A7s", 106);
        public static NoteKey B7b { get; } = new NoteKey("B7b", 106);
        public static NoteKey B7 { get; } = new NoteKey("B7", 107);

        public static NoteKey C8 { get; } = new NoteKey("C8", 108);      // piano highest note
        public static NoteKey C8s { get; } = new NoteKey("C8s", 109);
        public static NoteKey D8b { get; } = new NoteKey("D8b", 109);
        public static NoteKey D8 { get; } = new NoteKey("D8", 110);
        public static NoteKey D8s { get; } = new NoteKey("D8s", 111);
        public static NoteKey E8b { get; } = new NoteKey("E8b", 111);
        public static NoteKey E8 { get; } = new NoteKey("E8", 112);
        public static NoteKey F8 { get; } = new NoteKey("F8", 113);
        public static NoteKey F8s { get; } = new NoteKey("F8s", 114);
        public static NoteKey G8b { get; } = new NoteKey("G8b", 114);
        public static NoteKey G8 { get; } = new NoteKey("G8", 115);
        public static NoteKey G8s { get; } = new NoteKey("G8s", 116);
        public static NoteKey A8b { get; } = new NoteKey("A8b", 116);
        public static NoteKey A8 { get; } = new NoteKey("A8", 117);
        public static NoteKey A8s { get; } = new NoteKey("A8s", 118);
        public static NoteKey B8b { get; } = new NoteKey("B8b", 118);
        public static NoteKey B8 { get; } = new NoteKey("B8", 119);

        public static NoteKey C9 { get; } = new NoteKey("C9", 120);
        public static NoteKey C9s { get; } = new NoteKey("C9s", 121);
        public static NoteKey D9b { get; } = new NoteKey("D9b", 121);
        public static NoteKey D9 { get; } = new NoteKey("D9", 122);
        public static NoteKey D9s { get; } = new NoteKey("D9s", 123);
        public static NoteKey E9b { get; } = new NoteKey("E9b", 123);
        public static NoteKey E9 { get; } = new NoteKey("E9", 124);
        public static NoteKey F9 { get; } = new NoteKey("F9", 125);
        public static NoteKey F9s { get; } = new NoteKey("F9s", 126);
        public static NoteKey G9b { get; } = new NoteKey("G9b", 126);
        public static NoteKey G9 { get; } = new NoteKey("G9", 127);

        public static NoteKey[] NoteKeys = new NoteKey[]
        {
            Cm1,
            Cm1s,
            Dm1b,
            Dm1,
            Dm1s,
            Em1b,
            Em1,
            Fm1,
            Fm1s,
            Gm1b,
            Gm1,
            Gm1s,
            Am1b,
            Am1,
            Am1s,
            Bm1b,
            Bm1,

            C0,
            C0s,
            D0b,
            D0,
            D0s,
            E0b,
            E0,
            F0,
            F0s,
            G0b,
            G0,
            G0s,
            A0b,
            A0,
            A0s,
            B0b,
            B0,

            C1,
            C1s,
            D1b,
            D1,
            D1s,
            E1b,
            E1,
            F1,
            F1s,
            G1b,
            G1,
            G1s,
            A1b,
            A1,
            A1s,
            B1b,
            B1,

            C2,
            C2s,
            D2b,
            D2,
            D2s,
            E2b,
            E2,
            F2,
            F2s,
            G2b,
            G2,
            G2s,
            A2b,
            A2,
            A2s,
            B2b,
            B2,

            C3,
            C3s,
            D3b,
            D3,
            D3s,
            E3b,
            E3,
            F3,
            F3s,
            G3b,
            G3,
            G3s,
            A3b,
            A3,
            A3s,
            B3b,
            B3,

            C4,
            C4s,
            D4b,
            D4,
            D4s,
            E4b,
            E4,
            F4,
            F4s,
            G4b,
            G4,
            G4s,
            A4b,
            A4,
            A4s,
            B4b,
            B4,

            C5,
            C5s,
            D5b,
            D5,
            D5s,
            E5b,
            E5,
            F5,
            F5s,
            G5b,
            G5,
            G5s,
            A5b,
            A5,
            A5s,
            B5b,
            B5,

            C6,
            C6s,
            D6b,
            D6,
            D6s,
            E6b,
            E6,
            F6,
            F6s,
            G6b,
            G6,
            G6s,
            A6b,
            A6,
            A6s,
            B6b,
            B6,

            C7,
            C7s,
            D7b,
            D7,
            D7s,
            E7b,
            E7,
            F7,
            F7s,
            G7b,
            G7,
            G7s,
            A7b,
            A7,
            A7s,
            B7b,
            B7,

            C8,
            C8s,
            D8b,
            D8,
            D8s,
            E8b,
            E8,
            F8,
            F8s,
            G8b,
            G8,
            G8s,
            A8b,
            A8,
            A8s,
            B8b,
            B8,

            C9,
            C9s,
            D9b,
            D9,
            D9s,
            E9b,
            E9,
            F9,
            F9s,
            G9b,
            G9
        };
    }
}