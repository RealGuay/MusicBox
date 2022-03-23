using MusicBox.Services.Interfaces.MusicSheetModels;
using Prism.Ioc;
using Prism.Mvvm;

namespace MusicBox.Modules.SheetEditor.ViewModels
{
    public class BarEditorViewModel : BindableBase, IBarEditorViewModel
    {
        private readonly TimeSignature Default_TimeSignature = TimeSignature.TS_4_4;

        public TimeSignature TimeSignature { get; set; }

        public BarEditorViewModel()
        {
            TimeSignature = Default_TimeSignature;
        }

        public IBarEditorViewModel DeepCopy()
        {
            BarEditorViewModel newModel = new BarEditorViewModel();
            newModel.TimeSignature = TimeSignature;
            return newModel;
        }
    }
}