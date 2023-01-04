using MusicBox.Services.Interfaces;
using MusicBox.Services.Interfaces.MusicSheetModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task LoadAsync(SheetInformation sheetInformation)
        {
            string fileName = "MusicBox1.txt";

            bool? res = _dlgService.ShowOpenFileDialog(ref fileName, ".txt", "Text documents (.txt)|*.txt");
            if (res.HasValue && res.Value)
            {
                await ReadMusicFileAsync(sheetInformation, fileName);
            }
        }

        private async Task ReadMusicFileAsync(SheetInformation sheetInformation, string fileName)
        {
            using (StreamReader inputFile = new StreamReader(fileName, false))
            {
                sheetInformation.Title = inputFile.ReadLine();
                sheetInformation.LyricsBy = inputFile.ReadLine();
                sheetInformation.MusicBy = inputFile.ReadLine();
                sheetInformation.Version = inputFile.ReadLine();
                sheetInformation.Filename = fileName;
                await ReadSegmentsAsync(inputFile, sheetInformation);
            }
        }

        private async Task ReadSegmentsAsync(StreamReader inputFile, SheetInformation sheetInformation)
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
                await ReadBarsAsync(inputFile, segment);
                distinctSegments[segment.Name] = segment;
            }

            int totalSegmentCount = int.Parse(inputFile.ReadLine());
            for (int i = 0; i < totalSegmentCount; i++)
            {
                string segmentName = inputFile.ReadLine();
                sheetInformation.Segments.Add(distinctSegments[segmentName]);
            }
        }

        private async Task ReadBarsAsync(StreamReader inputFile, Segment segment)
        {
            int totalBarCount = int.Parse(inputFile.ReadLine());

            for (int i = 0; i < totalBarCount; i++)
            {
                Bar bar = new Bar();
                bar.Id = int.Parse(inputFile.ReadLine());
                bar.PlayOrder = int.Parse(inputFile.ReadLine());
                await ReadSheetNotesAsync(inputFile, bar);
                segment.Bars.Add(bar);
            }
        }

        private async Task ReadSheetNotesAsync(StreamReader inputFile, Bar bar)
        {
            int totalSheetNoteCount = int.Parse(inputFile.ReadLine());

            for (int i = 0; i < totalSheetNoteCount; i++)
            {
                SheetNote note = new SheetNote();
                note.Name = await inputFile.ReadLineAsync();
                note.Key = int.Parse(await inputFile.ReadLineAsync());
                note.PositionInBar = int.Parse(await inputFile.ReadLineAsync());
                note.Duration = int.Parse(await inputFile.ReadLineAsync());
                note.Volume = int.Parse(await inputFile.ReadLineAsync());
                note.Hand = (PlayingHand)Enum.Parse(typeof(PlayingHand), await inputFile.ReadLineAsync());
                bar.SheetNotes.Add(note);
            }
        }

        public async Task SaveAsync(SheetInformation sheetInformation)
        {
            string fileName = "MusicBox1.txt";

            if (!string.IsNullOrEmpty(sheetInformation.Filename))
            {
                fileName = sheetInformation.Filename;
            }

            bool? res = _dlgService.ShowSaveFileDialog(ref fileName, ".txt", "Text documents (.txt)|*.txt");
            if (res.HasValue && res.Value)
            {
                using (StreamWriter outputFile = new StreamWriter(fileName, false))
                {
                    await outputFile.WriteLineAsync(sheetInformation.Title);
                    await outputFile.WriteLineAsync(sheetInformation.LyricsBy);
                    await outputFile.WriteLineAsync(sheetInformation.MusicBy);
                    await outputFile.WriteLineAsync(sheetInformation.Version);
                    sheetInformation.Filename = fileName;
                    await WriteSegmentsAsync(outputFile, sheetInformation);
                }
            }
        }

        private async Task WriteSegmentsAsync(StreamWriter outputFile, SheetInformation sheetInformation)
        {
            int totalSegmentCount = sheetInformation.Segments.Count;
            int distinctSegmentCount = sheetInformation.Segments.Distinct().Count();

            await outputFile.WriteLineAsync(distinctSegmentCount.ToString());
            List<string> processedSegmentNames = new List<string>();
            foreach (Segment segment in sheetInformation.Segments)
            {
                if (!processedSegmentNames.Contains(segment.Name))
                {
                    processedSegmentNames.Add(segment.Name);
                    await outputFile.WriteLineAsync(segment.Name);
                    await outputFile.WriteLineAsync(segment.MidiChannel.ToString());
                    //outputFile.WriteLine(segment.Context);
                    await outputFile.WriteLineAsync(segment.TimeSignature.Name);
                    await outputFile.WriteLineAsync(segment.KeySignature.Name);
                    await WriteBarsAsync(outputFile, segment);
                }
            }

            await outputFile.WriteLineAsync(totalSegmentCount.ToString());
            foreach (Segment segment in sheetInformation.Segments)
            {
                await outputFile.WriteLineAsync(segment.Name);
            }
        }

        private async Task WriteBarsAsync(StreamWriter outputFile, Segment segment)
        {
            int totalBarCount = segment.Bars.Count;
            await outputFile.WriteLineAsync(totalBarCount.ToString());

            foreach (Bar bar in segment.Bars)
            {
                await outputFile.WriteLineAsync(bar.Id.ToString());
                await outputFile.WriteLineAsync(bar.PlayOrder.ToString());
                await WriteSheetNotesAsync(outputFile, bar);
            }
        }

        private async Task WriteSheetNotesAsync(StreamWriter outputFile, Bar bar)
        {
            int totalSheetNoteCount = bar.SheetNotes.Count;
            await outputFile.WriteLineAsync(totalSheetNoteCount.ToString());

            foreach (SheetNote note in bar.SheetNotes)
            {
                await outputFile.WriteLineAsync(note.Name);
                await outputFile.WriteLineAsync(note.Key.ToString());
                await outputFile.WriteLineAsync(note.PositionInBar.ToString());
                await outputFile.WriteLineAsync(note.Duration.ToString());
                await outputFile.WriteLineAsync(note.Volume.ToString());
                await outputFile.WriteLineAsync(note.Hand.ToString());
            }
        }
    }
}