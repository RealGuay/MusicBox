using Moq;
using MusicBox.Modules.SheetEditor.ViewModels;
using Xunit;

namespace MusicBox.Modules.SheetEditor.Tests.ViewModels
{
    public class SegmentCollectionViewModelTests
    {
        private readonly  string _initialName = "Segment1";
        private readonly SegmentCollectionViewModel _viewModel;


        public SegmentCollectionViewModelTests()
        {
            Mock<ISegmentEditorViewModel> sevm = new Mock<ISegmentEditorViewModel>();
            sevm.Setup(m => m.SegmentName).Returns(_initialName);
            
            _viewModel = new SegmentCollectionViewModel(() => sevm.Object);

        }

        [Fact]
        public void CreateOneSegmentEditorOnNewCommand()
        {
            _viewModel.NewSegmentCommand.Execute();

            Assert.Collection(_viewModel.SegmentEditorVms, item => Assert.Equal(_initialName, item.SegmentName));
            Assert.Equal(_initialName, _viewModel.SelectedSegmentEditorVm.SegmentName);
        }

    }
}