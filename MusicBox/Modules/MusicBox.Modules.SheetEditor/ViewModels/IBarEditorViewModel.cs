using MusicBox.Services.Interfaces.MusicSheetModels;

namespace MusicBox.Modules.SheetEditor.ViewModels
{
    public interface IBarEditorViewModel
    {
        TimeSignature TimeSignature { get; set; }

        IBarEditorViewModel DeepCopy();
    }
}