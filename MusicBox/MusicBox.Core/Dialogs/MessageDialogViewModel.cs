using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace MusicBox.Core.Dialogs
{
    public class MessageDialogViewModel : BindableBase, IDialogAware
    {
        private string title;
        public string Title { get => title; private set => SetProperty(ref title, value); }

        private string message;
        public string Message { get => message; private set => SetProperty(ref message, value); }

        public DelegateCommand YesCommand { get; set; }
        public DelegateCommand NoCommand { get; set; }

        public MessageDialogViewModel()
        {
            YesCommand = new DelegateCommand(YesResponseExecute);
            NoCommand = new DelegateCommand(NoResponseExecute);
        }

        public event Action<IDialogResult> RequestClose;

        private void YesResponseExecute()
        {
            IDialogResult result = new DialogResult(ButtonResult.Yes);
            RequestClose?.Invoke(result);
        }

        private void NoResponseExecute()
        {
            IDialogResult result = new DialogResult(ButtonResult.No);
            RequestClose?.Invoke(result);
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Title = parameters.GetValue<string>("title");
            Message = parameters.GetValue<string>("message");
        }
    }
}