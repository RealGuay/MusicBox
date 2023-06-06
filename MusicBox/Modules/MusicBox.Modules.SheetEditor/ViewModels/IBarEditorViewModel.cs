using MusicBox.Modules.SheetEditor.Models;
using MusicBox.Services.Interfaces.MusicSheetModels;
using Prism.Commands;
using System.Collections.Generic;
using static MusicBox.Services.Interfaces.MusicSheetModels.ScaleInformation;

namespace MusicBox.Modules.SheetEditor.ViewModels
{
    public interface IBarEditorViewModel
    {
        int TimePixelIncrement { get; }
        int TimePixelPerLine { get; set; }

        IBarEditorViewModel DeepCopy();

        void LoadBarInfo(Bar bar);
        void ExtractBarInfo(Bar currentBar, PlayingHand hand);
    }
}