using MusicBox.Core;
using MusicBox.Modules.Metronome.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace MusicBox.Modules.Metronome
{
    public class MetronomeModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public MetronomeModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RequestNavigate(RegionNames.ContentRegion, "SimpleMetronome");

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<SimpleMetronome>();
        }
    }
}