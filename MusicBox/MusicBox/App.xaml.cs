using MusicBox.Modules.ModuleName;
using MusicBox.Modules.Metronome;
using MusicBox.Services;
using MusicBox.Services.Interfaces;
using MusicBox.Services.MidiInterfaces;
using MusicBox.Services.MidiSanford;
using MusicBox.Services.Interfaces.Util;
using MusicBox.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;

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
            containerRegistry.RegisterSingleton<IBeatMaker, BeatMaker>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<ModuleNameModule>();
            moduleCatalog.AddModule<MetronomeModule>();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            IBeatMaker beatMaker = Container.Resolve<IBeatMaker>();
            beatMaker?.Dispose();
        }
    }
}