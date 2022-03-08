namespace MusicBox.Modules.SheetEditor.ViewModels
{
    public interface ISegmentEditorViewModel
    {
        string SegmentName { get; set; }
        ISegmentEditorViewModel DeepCopy();
    }
}