using MusicBox.Modules.ModuleName;
using MusicBox.Services;
using MusicBox.Services.Interfaces;
using MusicBox.Services.MidiInterfaces;
using MusicBox.Services.MidiSanford;
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
            containerRegistry.RegisterSingleton<IMidiTimer, MidiTimer>();
            containerRegistry.RegisterSingleton<IBeatMaker, BeatMaker>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<ModuleNameModule>();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            IBeatMaker beatMaker = Container.Resolve<IBeatMaker>();
            beatMaker?.Dispose();
        }
    }
}