using Microsoft.Win32;
using Win32.Services.Interfaces;

namespace Win32.Services
{
    public class Win32DialogsService : IWin32DialogsService
    {
        public bool? ShowOpenFileDialog(ref string filename, string extension, string filter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = filter;
            openFileDialog.FileName = filename;
            openFileDialog.Multiselect = false;
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            var res = openFileDialog.ShowDialog();
            if (res == true)
            {
                filename = openFileDialog.FileName;
            }
            return res;
        }

        public bool? ShowSaveFileDialog(ref string filename, string extension, string filter)
        {
            SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = filter,
                FileName = filename,
                OverwritePrompt = true,
                DefaultExt = extension
            };
            //            saveFileDialog.CheckFileExists = false;
            var res = saveFileDialog.ShowDialog();
            if (res == true)
            {
                filename = saveFileDialog.FileName;
            }
            return res;
        }
    }
}