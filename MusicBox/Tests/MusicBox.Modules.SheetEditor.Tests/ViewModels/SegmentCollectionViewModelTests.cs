using Moq;
using MusicBox.Modules.SheetEditor.ViewModels;
using Xunit;

namespace MusicBox.Modules.SheetEditor.Tests.ViewModels
{
    public class SegmentCollectionViewModelTests
    {
        private int _segmentCount = 1;
        private Mock<ISegmentEditorViewModel> _copySevm;
        private readonly string _initialName = "Segment";
        private readonly string _copyName = "SegmentTestCopy";
        private readonly SegmentCollectionViewModel _viewModel;

        public SegmentCollectionViewModelTests()
        {
            _copySevm = new Mock<ISegmentEditorViewModel>();
            _copySevm.Setup(m => m.SegmentName).Returns(_copyName);

            _viewModel = new SegmentCollectionViewModel(GetNewSevm);
        }

        private ISegmentEditorViewModel GetNewSevm()
        {
            Mock<ISegmentEditorViewModel> sevm = new Mock<ISegmentEditorViewModel>();
            sevm.Setup(m => m.SegmentName).Returns($"{_initialName}{_segmentCount++}");
            sevm.Setup(m => m.DeepCopy()).Returns(_copySevm.Object);
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
        public void MoveSegmentEditorUpOnEachMoveUpCommand()
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
        public void MoveSegmentEditorDownOnEachMoveDownCommand()
        {
            _viewModel.NewSegmentCommand.Execute();
            _viewModel.NewSegmentCommand.Execute();
            _viewModel.NewSegmentCommand.Execute();

            _viewModel.SelectedSegmentIndex = 0;
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
        public void MoveSegmentUpMoveSegmentDownButtonsDisabledOnViewModelCreation()
        {
            bool canMoveUp = _viewModel.MoveUpSegmentCommand.CanExecute();
            bool canMoveDown = _viewModel.MoveDownSegmentCommand.CanExecute();

            Assert.False(canMoveUp);
            Assert.False(canMoveDown);
        }

        [Fact]
        public void CallCanMoveDownSegmentOnSegmentSelectionChanges()  /// tttt
        {
            int canMoveDownCount = 0;
            _viewModel.MoveDownSegmentCommand.CanExecuteChanged += (sender, args) => canMoveDownCount++;

            _viewModel.NewSegmentCommand.Execute();
            _viewModel.NewSegmentCommand.Execute();
            _viewModel.NewSegmentCommand.Execute();

            _viewModel.SelectedSegmentIndex = 0;

            _viewModel.SelectedSegmentIndex = 1;

            _viewModel.MoveDownSegmentCommand.Execute();

            Assert.Equal(5, canMoveDownCount);
        }

        [Fact]
        public void CreateOneDeepCopyOnCopyCommand()
        {
            _viewModel.NewSegmentCommand.Execute();

            _viewModel.CopySegmentCommand.Execute();
            Assert.Collection(_viewModel.SegmentEditorVms,
                item => Assert.Equal($"{_initialName}{1}", item.SegmentName),
                item => Assert.Equal(_copyName, item.SegmentName)
                );
            Assert.Equal(_copyName, _viewModel.SelectedSegmentEditorVm.SegmentName);
        }

        [Fact]
        public void AddTheSameSegmentOnRepeatSegmentCommand()
        {
            _viewModel.NewSegmentCommand.Execute();
            _viewModel.NewSegmentCommand.Execute();
            _viewModel.NewSegmentCommand.Execute();

            _viewModel.SelectedSegmentIndex = 1;

            _viewModel.RepeatSegmentCommand.Execute();

            Assert.Collection(_viewModel.SegmentEditorVms,
                                item => Assert.Equal($"{_initialName}{1}", item.SegmentName),
                                item => Assert.Equal($"{_initialName}{2}", item.SegmentName),
                                item => Assert.Equal($"{_initialName}{3}", item.SegmentName),
                                item => Assert.Equal($"{_initialName}{2}", item.SegmentName));
            Assert.Equal($"{_initialName}{2}", _viewModel.SelectedSegmentEditorVm.SegmentName);
        }
    }
}