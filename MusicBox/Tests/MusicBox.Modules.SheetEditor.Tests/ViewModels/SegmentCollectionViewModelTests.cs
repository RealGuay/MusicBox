using Moq;
using MusicBox.Modules.SheetEditor.ViewModels;
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

            var viewModel = new SegmentCollectionViewModel(() => sevm.Object);
            viewModel.NewSegmentCommand.Execute(null);

            Assert.Collection(viewModel.SegmentEditorVms, item => Assert.Equal(name, item.SegmentName));
            Assert.Equal(name, viewModel.SelectedSegmentEditorVm.SegmentName);
        }
    }
}