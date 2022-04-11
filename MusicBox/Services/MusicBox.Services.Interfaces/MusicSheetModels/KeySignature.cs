using System.Collections.Generic;
using static MusicBox.Services.Interfaces.MusicSheetModels.ScaleInformation;

namespace MusicBox.Services.Interfaces.MusicSheetModels
{
    public class KeySignature
    {
        public string Name { get; }
        public BarAlteration BarAlteration { get; }

        private KeySignature(string name, BarAlteration barAlteration)
        {
            Name = name;
            BarAlteration = barAlteration;
        }

        public static KeySignature Natural { get; } = new KeySignature("Natural", BarAlteration.None);

        public static KeySignature OneFlat { get; } = new KeySignature("Bb", BarAlteration.Flat1);
        public static KeySignature TwoFlat { get; } = new KeySignature("Bb Eb", BarAlteration.Flat2);
        public static KeySignature ThreeFlat { get; } = new KeySignature("Bb Eb Ab", BarAlteration.Flat3);
        public static KeySignature FourFlat { get; } = new KeySignature("Bb Eb Ab Db", BarAlteration.Flat4);
        public static KeySignature FiveFlat { get; } = new KeySignature("Bb Eb Ab Db Gb", BarAlteration.Flat5);

        public static KeySignature OneSharp { get; } = new KeySignature("F#", BarAlteration.Sharp1);
        public static KeySignature TwoSharp { get; } = new KeySignature("F# C#", BarAlteration.Sharp2);
        public static KeySignature ThreeSharp { get; } = new KeySignature("F# C# G#", BarAlteration.Sharp3);
        public static KeySignature FourSharp { get; } = new KeySignature("F# C# G# D# ", BarAlteration.Sharp4);
        public static KeySignature FiveSharp { get; } = new KeySignature("F# C# G# D# A#", BarAlteration.Sharp5);

        public static List<KeySignature> AllKeySignatures = new List<KeySignature>()
        {
            Natural,
            OneFlat,
            TwoFlat,
            ThreeFlat,
            FourFlat,
            FiveFlat,
            OneSharp,
            TwoSharp,
            ThreeSharp,
            FourSharp,
            FiveSharp
        };
    }
}