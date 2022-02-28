using MusicBox.Core;
using MusicBox.Modules.SheetEditor.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace MusicBox.Modules.SheetEditor
{
    public class SheetEditorModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public SheetEditorModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RequestNavigate(RegionNames.ContentRegion, "SheetEditorView");
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<SheetEditorView>();
        }
    }
}