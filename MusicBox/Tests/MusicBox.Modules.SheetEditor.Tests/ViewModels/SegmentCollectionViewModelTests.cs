using Moq;
using MusicBox.Modules.SheetEditor.ViewModels;
using Xunit;

namespace MusicBox.Modules.SheetEditor.Tests.ViewModels
{
    public class SegmentCollectionViewModelTests
    {
        private int _segmentCount = 1;
        private Mock<ISegmentEditorViewModel> _repeatSevm;
        private readonly string _initialName = "Segment";
        private readonly string _repeatName = "SegmentRepeat1";
        private readonly SegmentCollectionViewModel _viewModel;

        public SegmentCollectionViewModelTests()
        {
            _repeatSevm = new Mock<ISegmentEditorViewModel>();
            _repeatSevm.Setup(m => m.SegmentName).Returns(_repeatName);

            _viewModel = new SegmentCollectionViewModel(GetNewSevm);
        }

        private ISegmentEditorViewModel GetNewSevm()
        {
            Mock<ISegmentEditorViewModel> sevm = new Mock<ISegmentEditorViewModel>();
            sevm.Setup(m => m.SegmentName).Returns($"{_initialName}{_segmentCount++}");
            sevm.Setup(m => m.DeepCopy()).Returns(_repeatSevm.Object);
            return sevm.Object;
        }

        [Fact]
        public void CreateOneSegmentEditorOnNewCommand()
        {
            _viewModel.NewSegmentCommand.Execute();

            Assert.Collection(_viewModel.SegmentEditorVms, item => Assert.Equal($"{_initialName}{1}", item.SegmentName));
            Assert.Equal($"{_initialName}{1}", _viewModel.SelectedSegmentEditorVm.SegmentName);
        }

        [Fact]
        public void CreateOneSegmentEditorOnEachNewCommand()  // the create object is a Mock and it is always the same instance !!!
        {
            _viewModel.NewSegmentCommand.Execute();
            _viewModel.NewSegmentCommand.Execute();
            _viewModel.NewSegmentCommand.Execute();

            Assert.Collection(_viewModel.SegmentEditorVms,
                item => Assert.Equal($"{_initialName}{1}", item.SegmentName),
                item => Assert.Equal($"{_initialName}{2}", item.SegmentName),
                item => Assert.Equal($"{_initialName}{3}", item.SegmentName));
            Assert.Equal($"{_initialName}{3}", _viewModel.SelectedSegmentEditorVm.SegmentName);
        }

        [Fact]
        public void MoveUpSegmentEditorOnEachMoveUpCommand() 
        {
            _viewModel.NewSegmentCommand.Execute();
            _viewModel.NewSegmentCommand.Execute();
            _viewModel.NewSegmentCommand.Execute();

            _viewModel.MoveUpSegmentCommand.Execute();
            Assert.Collection(_viewModel.SegmentEditorVms,
                item => Assert.Equal($"{_initialName}{1}", item.SegmentName),
                item => Assert.Equal($"{_initialName}{3}", item.SegmentName),
                item => Assert.Equal($"{_initialName}{2}", item.SegmentName));
            Assert.Equal($"{_initialName}{3}", _viewModel.SelectedSegmentEditorVm.SegmentName);

            _viewModel.MoveUpSegmentCommand.Execute();
            Assert.Collection(_viewModel.SegmentEditorVms,
                item => Assert.Equal($"{_initialName}{3}", item.SegmentName),
                item => Assert.Equal($"{_initialName}{1}", item.SegmentName),
                item => Assert.Equal($"{_initialName}{2}", item.SegmentName));
            Assert.Equal($"{_initialName}{3}", _viewModel.SelectedSegmentEditorVm.SegmentName);

            _viewModel.MoveUpSegmentCommand.Execute();
            Assert.Collection(_viewModel.SegmentEditorVms,
                item => Assert.Equal($"{_initialName}{3}", item.SegmentName),
                item => Assert.Equal($"{_initialName}{1}", item.SegmentName),
                item => Assert.Equal($"{_initialName}{2}", item.SegmentName));
            Assert.Equal($"{_initialName}{3}", _viewModel.SelectedSegmentEditorVm.SegmentName);
        }

        [Fact]
        public void MoveDownSegmentEditorOnEachMoveDownCommand() 
        {
            _viewModel.NewSegmentCommand.Execute();
            _viewModel.NewSegmentCommand.Execute();
            _viewModel.NewSegmentCommand.Execute();

            _viewModel.SelectedSegmentEditorVm = _viewModel.SegmentEditorVms[0];
            Assert.Equal($"{_initialName}{1}", _viewModel.SelectedSegmentEditorVm.SegmentName);

            _viewModel.MoveDownSegmentCommand.Execute();
            Assert.Collection(_viewModel.SegmentEditorVms,
                item => Assert.Equal($"{_initialName}{2}", item.SegmentName),
                item => Assert.Equal($"{_initialName}{1}", item.SegmentName),
                item => Assert.Equal($"{_initialName}{3}", item.SegmentName));
            Assert.Equal($"{_initialName}{1}", _viewModel.SelectedSegmentEditorVm.SegmentName);

            _viewModel.MoveDownSegmentCommand.Execute();
            Assert.Collection(_viewModel.SegmentEditorVms,
                item => Assert.Equal($"{_initialName}{2}", item.SegmentName),
                item => Assert.Equal($"{_initialName}{3}", item.SegmentName),
                item => Assert.Equal($"{_initialName}{1}", item.SegmentName));
            Assert.Equal($"{_initialName}{1}", _viewModel.SelectedSegmentEditorVm.SegmentName);

            _viewModel.MoveDownSegmentCommand.Execute();
            Assert.Collection(_viewModel.SegmentEditorVms,
                item => Assert.Equal($"{_initialName}{2}", item.SegmentName),
                item => Assert.Equal($"{_initialName}{3}", item.SegmentName),
                item => Assert.Equal($"{_initialName}{1}", item.SegmentName));
            Assert.Equal($"{_initialName}{1}", _viewModel.SelectedSegmentEditorVm.SegmentName);
        }

        [Fact]
        public void CreateOneDeepCopyOnRepeatCommand()
        {
            _viewModel.NewSegmentCommand.Execute();

            _viewModel.RepeatSegmentCommand.Execute();
            Assert.Collection(_viewModel.SegmentEditorVms, 
                item => Assert.Equal($"{_initialName}{1}", item.SegmentName),
                item => Assert.Equal(_repeatName, item.SegmentName)
                );
            Assert.Equal(_repeatName, _viewModel.SelectedSegmentEditorVm.SegmentName);
        }

    }
    }