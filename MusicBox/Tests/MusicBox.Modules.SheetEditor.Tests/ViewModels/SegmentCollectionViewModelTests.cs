using Moq;
using MusicBox.Modules.SheetEditor.ViewModels;
using Prism.Services.Dialogs;
using System;
using Xunit;

namespace MusicBox.Modules.SheetEditor.Tests.ViewModels
{
    public class SegmentCollectionViewModelTests
    {
        private int _segmentCount = 1;
        private Mock<ISegmentEditorViewModel> _copySevm;
        private readonly string _initialName = "Segment";
        private readonly string _copyName = "SegmentTestCopied";
        private readonly SegmentCollectionViewModel _viewModel;
        private readonly Mock<IDialogService> _dialogService;

        public SegmentCollectionViewModelTests()
        {
            _copySevm = new Mock<ISegmentEditorViewModel>();
            _copySevm.Setup(m => m.SegmentName).Returns(_copyName);

            _dialogService = new Mock<IDialogService>();
            _viewModel = new SegmentCollectionViewModel(GetNewSevm, _dialogService.Object);

            //            _dialogService.Setup(m => m.ShowDialog(It.IsAny<string>(), It.IsAny<DialogParameters>(),It.IsAny<Action<IDialogResult>>()));
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

        [Theory]
        [InlineData(0, -1, false, false)]
        [InlineData(1, 0, false, false)]
        [InlineData(2, 0, false, true)]
        [InlineData(2, 1, true, false)]
        [InlineData(3, 0, false, true)]
        [InlineData(3, 1, true, true)]
        [InlineData(3, 2, true, false)]
        public void EnableMoveUpMoveDownButtonsOnSegmentSelection(int segmentsToCreate, int selectSegmentIndex, bool canUp, bool canDown)
        {
            for (int i = 0; i < segmentsToCreate; i++)
            {
                _viewModel.NewSegmentCommand.Execute();
            }

            _viewModel.SelectedSegmentIndex = selectSegmentIndex;

            bool canMoveUp = _viewModel.MoveUpSegmentCommand.CanExecute();
            bool canMoveDown = _viewModel.MoveDownSegmentCommand.CanExecute();

            Assert.Equal(canUp, canMoveUp);
            Assert.Equal(canDown, canMoveDown);
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
        }

        // StaFact is required because the RaiseCanExecuteChanged of Prism uses another thread to call the delegate in CanExecuteChanged
        /* from Prism.Commands DelegateCommandBase
        // <summary>
        /// Raises <see cref="ICommand.CanExecuteChanged"/> so every
        /// command invoker can requery <see cref="ICommand.CanExecute"/>.
        /// </summary>
        protected virtual void OnCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                if (_synchronizationContext != null && _synchronizationContext != SynchronizationContext.Current)
                    _synchronizationContext.Post((o) => handler.Invoke(this, EventArgs.Empty), null); // <<<<<<<<<<   Post   <<<<<<<<<<<<<<<<<
                else
                    handler.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises <see cref="CanExecuteChanged"/> so every command invoker
        /// can requery to check if the command can execute.
        /// </summary>
        /// <remarks>Note that this will trigger the execution of <see cref="CanExecuteChanged"/> once for each invoker.</remarks>
        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }
        */

        // StaFact ensures that everything runs on the same thread
        [StaFact]
        public void CallCanMoveUpDownSegmentOnSegmentSelectionChanges()
        {
            int canMoveUpCount = 0;
            int canMoveDownCount = 0;
            _viewModel.NewSegmentCommand.Execute();
            _viewModel.NewSegmentCommand.Execute();
            _viewModel.NewSegmentCommand.Execute();
            _viewModel.MoveUpSegmentCommand.CanExecuteChanged += (sender, args) => canMoveUpCount++;
            _viewModel.MoveDownSegmentCommand.CanExecuteChanged += (sender, args) => canMoveDownCount++;

            _viewModel.SelectedSegmentIndex = 0;
            Assert.Equal(1, canMoveUpCount);
            Assert.Equal(1, canMoveDownCount);

            _viewModel.SelectedSegmentIndex = 1;
            Assert.Equal(2, canMoveUpCount);
            Assert.Equal(2, canMoveDownCount);

            _viewModel.SelectedSegmentIndex = 2;
            Assert.Equal(3, canMoveUpCount);
            Assert.Equal(3, canMoveDownCount);

            _viewModel.SelectedSegmentIndex = 1;
            Assert.Equal(4, canMoveUpCount);
            Assert.Equal(4, canMoveDownCount);

            _viewModel.SelectedSegmentIndex = 0;
            Assert.Equal(5, canMoveUpCount);
            Assert.Equal(5, canMoveDownCount);
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

        [Fact]
        public void CanMoveTheSameSegmentDependingOfItsPosition()
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

            bool canMoveUp = _viewModel.MoveUpSegmentCommand.CanExecute();
            bool canMoveDown = _viewModel.MoveDownSegmentCommand.CanExecute();
            Assert.True(canMoveUp);
            Assert.False(canMoveDown);

            _viewModel.SelectedSegmentIndex = 1;
            canMoveUp = _viewModel.MoveUpSegmentCommand.CanExecute();
            canMoveDown = _viewModel.MoveDownSegmentCommand.CanExecute();
            Assert.True(canMoveUp);
            Assert.True(canMoveDown);
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
        public void CanMoveTheCopiedSegmentDependingOfItsPosition()
        {
            _viewModel.NewSegmentCommand.Execute();
            _viewModel.NewSegmentCommand.Execute();
            _viewModel.NewSegmentCommand.Execute();

            _viewModel.SelectedSegmentIndex = 1;

            _viewModel.CopySegmentCommand.Execute();

            Assert.Collection(_viewModel.SegmentEditorVms,
                                item => Assert.Equal($"{_initialName}{1}", item.SegmentName),
                                item => Assert.Equal($"{_initialName}{2}", item.SegmentName),
                                item => Assert.Equal($"{_initialName}{3}", item.SegmentName),
                                item => Assert.Equal(_copyName, item.SegmentName));
            Assert.Equal(_copyName, _viewModel.SelectedSegmentEditorVm.SegmentName);

            bool canMoveUp = _viewModel.MoveUpSegmentCommand.CanExecute();
            bool canMoveDown = _viewModel.MoveDownSegmentCommand.CanExecute();
            Assert.True(canMoveUp);
            Assert.False(canMoveDown);

            _viewModel.SelectedSegmentIndex = 1;
            canMoveUp = _viewModel.MoveUpSegmentCommand.CanExecute();
            canMoveDown = _viewModel.MoveDownSegmentCommand.CanExecute();
            Assert.True(canMoveUp);
            Assert.True(canMoveDown);
        }

        [Fact]
        public void RemoveSegmentEditorUpOnDeleteCommand()
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

            _viewModel.SelectedSegmentIndex = 1;
            _viewModel.DeleteSegmentCommand.Execute();

            Assert.Collection(_viewModel.SegmentEditorVms,
                                item => Assert.Equal($"{_initialName}{1}", item.SegmentName),
                                item => Assert.Equal($"{_initialName}{3}", item.SegmentName),
                                item => Assert.Equal($"{_initialName}{2}", item.SegmentName));
        }

        [Fact]
        private void ShowYesNoDialogOnRemoveLastInstanceOfTheSelectedSegment()
        {
            _viewModel.NewSegmentCommand.Execute();
            _viewModel.NewSegmentCommand.Execute();
            _viewModel.NewSegmentCommand.Execute();

            Assert.Collection(_viewModel.SegmentEditorVms,
                                item => Assert.Equal($"{_initialName}{1}", item.SegmentName),
                                item => Assert.Equal($"{_initialName}{2}", item.SegmentName),
                                item => Assert.Equal($"{_initialName}{3}", item.SegmentName));

            _viewModel.SelectedSegmentIndex = 1;

            IDialogResult result = new DialogResult(ButtonResult.Yes);
            _dialogService.Setup(m => m.ShowDialog(It.IsAny<string>(), It.IsAny<DialogParameters>(), It.IsAny<Action<IDialogResult>>()))
                            .Callback<string, IDialogParameters, Action<IDialogResult>>((s, p, a) => a.Invoke(result));

            _viewModel.DeleteSegmentCommand.Execute();

            Assert.Collection(_viewModel.SegmentEditorVms,
                    item => Assert.Equal($"{_initialName}{1}", item.SegmentName),
                    item => Assert.Equal($"{_initialName}{3}", item.SegmentName));
        }
    }
}