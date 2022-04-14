using MusicBox.Services.Interfaces;
using MusicBox.Services.Interfaces.MusicSheetModels;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Win32.Services.Interfaces;

namespace MusicBox.Services
{
    public class TextFileSheetInfoRepo : ISheetInformationRepo
    {
        private readonly IWin32DialogsService _dlgService;

        public TextFileSheetInfoRepo(IWin32DialogsService win32DialogService)
        {
            _dlgService = win32DialogService;
        }

        public void Load(SheetInformation sheetInformation)
        {
            string fileName = "MusicBox1.txt";
            bool? res = _dlgService.ShowOpenFileDialog(ref fileName, ".txt", "Text documents (.txt)|*.txt");

            if (res.HasValue && res.Value)
            {
                using (StreamReader inputFile = new StreamReader(fileName, false))
                {
                    sheetInformation.Title = inputFile.ReadLine();
                    sheetInformation.LyricsBy = inputFile.ReadLine();
                    sheetInformation.MusicBy = inputFile.ReadLine();
                    sheetInformation.Version = inputFile.ReadLine();
                    ReadSegments(inputFile, sheetInformation);
                }
            }
        }

        private void ReadSegments(StreamReader inputFile, SheetInformation sheetInformation)
        {
            Dictionary<string, Segment> distinctSegments = new Dictionary<string, Segment>();

            int distinctSegmentCount = int.Parse(inputFile.ReadLine());

            for (int i = 0; i < distinctSegmentCount; i++)
            {
                var segment = new Segment();
                segment.Name = inputFile.ReadLine();
                segment.MidiChannel = int.Parse(inputFile.ReadLine());
                //outputFile.WriteLine(segment.Context);
                segment.TimeSignature = TimeSignature.FromName(inputFile.ReadLine());
                segment.KeySignature = KeySignature.FromName(inputFile.ReadLine());
                ReadBars(inputFile, segment);
                distinctSegments[segment.Name] = segment;
            }

            int totalSegmentCount = int.Parse(inputFile.ReadLine());
            for (int i = 0; i < totalSegmentCount; i++)
            {
                string segmentName = inputFile.ReadLine();
                sheetInformation.Segments.Add(distinctSegments[segmentName]);
            }
        }

        private void ReadBars(StreamReader inputFile, Segment segment)
        {
            int totalBarCount = int.Parse(inputFile.ReadLine());

            for (int i = 0; i < totalBarCount; i++)
            {
                Bar bar = new Bar();
                bar.Id = int.Parse(inputFile.ReadLine());
                bar.PlayOrder = int.Parse(inputFile.ReadLine());
                ReadSheetNotes(inputFile, bar);
                segment.Bars.Add(bar);
            }
        }

        private void ReadSheetNotes(StreamReader inputFile, Bar bar)
        {
            int totalSheetNoteCount = int.Parse(inputFile.ReadLine());

            for (int i = 0; i < totalSheetNoteCount; i++)
            {
                SheetNote note = new SheetNote();
                note.Name = inputFile.ReadLine();
                note.Key = int.Parse(inputFile.ReadLine());
                note.PositionInBar = int.Parse(inputFile.ReadLine());
                note.Duration = int.Parse(inputFile.ReadLine());
                note.Volume = int.Parse(inputFile.ReadLine());
                bar.SheetNotes.Add(note);
            }
        }

        public void Save(SheetInformation sheetInformation)
        {
            string fileName = "MusicBox1.txt";

            bool? res = _dlgService.ShowSaveFileDialog(ref fileName, ".txt", "Text documents (.txt)|*.txt");
            if (res.HasValue && res.Value)
            {
                using (StreamWriter outputFile = new StreamWriter(fileName, false))
                {
                    outputFile.WriteLine(sheetInformation.Title);
                    outputFile.WriteLine(sheetInformation.LyricsBy);
                    outputFile.WriteLine(sheetInformation.MusicBy);
                    outputFile.WriteLine(sheetInformation.Version);
                    WriteSegments(outputFile, sheetInformation);
                }
            }
        }

        private void WriteSegments(StreamWriter outputFile, SheetInformation sheetInformation)
        {
            int totalSegmentCount = sheetInformation.Segments.Count;
            int distinctSegmentCount = sheetInformation.Segments.Distinct().Count();

            outputFile.WriteLine(distinctSegmentCount);
            List<string> processedSegmentNames = new List<string>();
            foreach (Segment segment in sheetInformation.Segments)
            {
                if (!processedSegmentNames.Contains(segment.Name))
                {
                    processedSegmentNames.Add(segment.Name);
                    outputFile.WriteLine(segment.Name);
                    outputFile.WriteLine(segment.MidiChannel);
                    //outputFile.WriteLine(segment.Context);
                    outputFile.WriteLine(segment.TimeSignature.Name);
                    outputFile.WriteLine(segment.KeySignature.Name);
                    WriteBars(outputFile, segment);
                }
            }

            outputFile.WriteLine(totalSegmentCount);
            foreach (Segment segment in sheetInformation.Segments)
            {
                outputFile.WriteLine(segment.Name);
            }
        }

        private void WriteBars(StreamWriter outputFile, Segment segment)
        {
            int totalBarCount = segment.Bars.Count;
            outputFile.WriteLine(totalBarCount);

            foreach (Bar bar in segment.Bars)
            {
                outputFile.WriteLine(bar.Id);
                outputFile.WriteLine(bar.PlayOrder);
                WriteSheetNotes(outputFile, bar);
            }
        }

        private void WriteSheetNotes(StreamWriter outputFile, Bar bar)
        {
            int totalSheetNoteCount = bar.SheetNotes.Count;
            outputFile.WriteLine(totalSheetNoteCount);

            foreach (SheetNote note in bar.SheetNotes)
            {
                outputFile.WriteLine(note.Name);
                outputFile.WriteLine(note.Key);
                outputFile.WriteLine(note.PositionInBar);
                outputFile.WriteLine(note.Duration);
                outputFile.WriteLine(note.Volume);
            }
        }
    }
}