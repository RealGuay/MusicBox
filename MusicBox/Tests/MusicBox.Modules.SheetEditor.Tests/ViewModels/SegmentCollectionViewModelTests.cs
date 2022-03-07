using Moq;
using MusicBox.Modules.SheetEditor.ViewModels;
using Prism.Ioc;
using Xunit;

namespace MusicBox.Modules.SheetEditor.Tests.ViewModels
{
    public class SegmentCollectionViewModelTests
    {
        [Fact]
        public void CreateOneSegmentEditorOnNewCommand()
        {
            string name = "Segment1";
            var sevm = new Mock<ISegmentEditorViewModel>();
            sevm.Setup(m => m.SegmentName).Returns(name);

            var provider = new Mock<IContainerProvider>();
            // provider.Setup(p => p.Resolve(typeof(SegmentEditorViewModel))).Returns(sevm);
            // provider.Setup(provider => provider.Resolve<ISegmentEditorViewModel>()).Returns(sevm.Object);
            provider.Setup(provider => provider.Resolve(typeof(ISegmentEditorViewModel))).Returns(sevm.Object);


            var viewModel = new SegmentCollectionViewModel(provider.Object);
            viewModel.NewSegmentCommand.Execute(null);

            Assert.Collection(viewModel.SegmentEditorVms, item => Assert.Equal(name, item.SegmentName));
            Assert.Equal(name, viewModel.SelectedSegmentEditorVm.SegmentName);
        }
    }
}