

namespace Win32.Services.Interfaces
{
    public interface IWin32DialogsService
    {
        bool? ShowOpenFileDialog(ref string filename, string extension, string filter);
        bool? ShowSaveFileDialog(ref string filename, string extension, string filter);
    }
}