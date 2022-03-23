using Moq;
using MusicBox.Modules.SheetEditor.ViewModels;
using System;
using Xunit;

namespace MusicBox.Modules.SheetEditor.Tests.ViewModels
{
    public class SegmentEditorViewModelTests
    {
        private readonly SegmentEditorViewModel _viewModel;

        public SegmentEditorViewModelTests()
        {
            _viewModel = new SegmentEditorViewModel(CreateBarEditorVm);
        }

        private BarEditorViewModel CreateBarEditorVm()
        {
            return new Mock<BarEditorViewModel>().Object;
        }

        [Fact]
        public void AddBarEditorOnAddBarCommand()
        {
            Assert.Empty(_viewModel.BarEditorVms);

            _viewModel.AddBarCommand.Execute();

            Assert.Single(_viewModel.BarEditorVms);
        }

        [Fact]
        public void DeleteBarOnDeleteBarCommand()
        {
            _viewModel.AddBarCommand.Execute();
            _viewModel.AddBarCommand.Execute();

            _viewModel.DeleteBarCommand.Execute();

            Assert.Single(_viewModel.BarEditorVms);

            _viewModel.DeleteBarCommand.Execute();

            Assert.Empty(_viewModel.BarEditorVms);
        }

        [Fact]
        public void CopyEachBarEditorOnDeepCopy()
        {
            _viewModel.AddBarCommand.Execute();
            _viewModel.AddBarCommand.Execute();
            _viewModel.AddBarCommand.Execute();
            _viewModel.AddBarCommand.Execute();

            var copy = _viewModel.DeepCopy();

            Assert.Equal(4, _viewModel.BarEditorVms.Count);
            Assert.Equal(4, copy.BarEditorVms.Count);
        }
    }
}