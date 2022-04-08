using MusicBox.Modules.SheetEditor.Models;
using MusicBox.Services.Interfaces.MusicSheetModels;
using Prism.Commands;
using System.Collections.Generic;
using static MusicBox.Services.Interfaces.MusicSheetModels.ScaleInformation;

namespace MusicBox.Modules.SheetEditor.ViewModels
{
    public interface IBarEditorViewModel
    {
        DelegateCommand<TimePixel> ActivatePixelCommand { get; set; }
        DelegateCommand<TimePixel> AlterPixelCommand { get; set; }
        BarAlteration BarAlteration { get; set; }
        DelegateCommand<TimePixel> ExpandPixelCommand { get; set; }
        TimePixel SelectedPixel { get; set; }
        int TimePixelIncrement { get; }
        int TimePixelPerLine { get; set; }
        List<TimePixel> TimePixels { get; set; }

        IBarEditorViewModel DeepCopy();
        void LoadBarInfo(Bar bar);
    }
}