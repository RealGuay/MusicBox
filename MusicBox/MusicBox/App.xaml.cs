using MusicBox.Core.Dialogs;
using MusicBox.Modules.Metronome;
using MusicBox.Modules.ModuleName;
using MusicBox.Modules.SheetEditor;
using MusicBox.Services;
using MusicBox.Services.Interfaces;
using MusicBox.Services.Interfaces.Util;
using MusicBox.Services.MidiInterfaces;
using MusicBox.Services.MidiSanford;
using MusicBox.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;
using Win32.Services;
using Win32.Services.Interfaces;

namespace MusicBox
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IMessageService, MessageService>();

            containerRegistry.Register<WrapAroundCounter>();
            containerRegistry.RegisterSingleton<IMidiTimer, MidiTimer>();
            containerRegistry.RegisterSingleton<IMidiOutputDevice, MidiOutputDevice>();
            containerRegistry.RegisterSingleton<IBeatMaker, BeatMaker>();
            containerRegistry.RegisterSingleton<IMidiPlayer, MidiPlayer>();
            containerRegistry.RegisterDialog<MessageDialog, MessageDialogViewModel>();
            containerRegistry.RegisterSingleton<ISheetInformationRepo, TextFileSheetInfoRepo>();
            containerRegistry.RegisterSingleton<IWin32DialogsService, Win32DialogsService>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<ModuleNameModule>();
            moduleCatalog.AddModule<MetronomeModule>();
            moduleCatalog.AddModule<SheetEditorModule>();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            IBeatMaker beatMaker = Container.Resolve<IBeatMaker>();
            beatMaker?.Dispose();

            IMidiOutputDevice outputDevice = Container.Resolve<IMidiOutputDevice>();
            outputDevice?.Dispose();
        }
    }
}