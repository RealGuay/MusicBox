using MusicBox.Services.Interfaces.MusicSheetModels;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Runtime.CompilerServices;

namespace MusicBox.Modules.SheetEditor.Models
{
    public class SheetInfo : BindableBase
    {
        private readonly SheetInformation _sheetInformation;

        #region Properties

        public string Title
        {
            get { return _sheetInformation.Title; }
            set { ChangeProperty(_sheetInformation.Title, value, value => _sheetInformation.Title = value); }
        }

        public string LyricsBy
        {
            get { return _sheetInformation.LyricsBy; }
            set { ChangeProperty(_sheetInformation.LyricsBy, value, value => _sheetInformation.LyricsBy = value); }
        }

        public string MusicBy
        {
            get { return _sheetInformation.MusicBy; }
            set { ChangeProperty(_sheetInformation.MusicBy, value, value => _sheetInformation.MusicBy = value); }
        }

        public string Version
        {
            get { return _sheetInformation.Version; }
            set { ChangeProperty(_sheetInformation.Version, value, value => _sheetInformation.Version = value); }
        }

        public TimeSignature TimeSignature
        {
            get { return _sheetInformation.Context.TimeSignature; }
            set { ChangeProperty(_sheetInformation.Context.TimeSignature, value, value => _sheetInformation.Context.TimeSignature = value); }
        }

        public  int Tempo
        {
            get { return _sheetInformation.Context.Tempo; }
            set { ChangeProperty(_sheetInformation.Context.Tempo, value, value => _sheetInformation.Context.Tempo = value); }
        }

        #endregion Properties

        public SheetInfo(IContainerProvider containerProvider)
        {
            _sheetInformation = containerProvider.Resolve<SheetInformation>();
        }

        private void ChangeProperty<T>(T current, T value, Action<T> setOutput, [CallerMemberName] string propertyName = null)
        {
            T dummy = current;
            setOutput(value);
            SetProperty(ref dummy, value, propertyName);
        }
    }
}