using MusicBox.Services.Interfaces.MusicSheetModels;
using Prism.Commands;
using System.Collections.ObjectModel;

namespace MusicBox.Modules.SheetEditor.ViewModels
{
    public interface ISegmentEditorViewModel
    {
        ObservableCollection<IBarEditorViewModel> BarEditorVms { get; set; }

        DelegateCommand AddBarCommand { get; set; }
        DelegateCommand DeleteBarCommand { get; set; }

        string SegmentName { get; set; }

        IBarEditorViewModel SelectedBarEditorVm { get; set; }

        ISegmentEditorViewModel DeepCopy();

        void ExtractSegmentInfo(Segment currentSegment, PlayingHand hand);
        void ExtractSelectedBarInfo(Segment segment, PlayingHand hand);
        void LoadSegmentInfo(Segment segment);
    }
}