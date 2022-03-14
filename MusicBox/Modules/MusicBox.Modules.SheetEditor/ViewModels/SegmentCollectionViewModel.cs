using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace MusicBox.Modules.SheetEditor.ViewModels
{
    public class SegmentCollectionViewModel : BindableBase
    {
        #region Properties

        private ObservableCollection<ISegmentEditorViewModel> segmentEditorVms;

        public ObservableCollection<ISegmentEditorViewModel> SegmentEditorVms { get => segmentEditorVms; set => SetProperty(ref segmentEditorVms, value); }

        private ISegmentEditorViewModel selectedSegmentEditorVm;

        public ISegmentEditorViewModel SelectedSegmentEditorVm { get => selectedSegmentEditorVm; set => SetProperty(ref selectedSegmentEditorVm, value); }

        private int selectedSegmentIndex;
        public int SelectedSegmentIndex { get => selectedSegmentIndex; set => ChangeSelectedSegmentIndex(ref selectedSegmentIndex, value); }

        #endregion Properties

        private readonly Func<ISegmentEditorViewModel> _segmentEditorViewModelCreator;

        #region DelegateCommands

        public DelegateCommand NewSegmentCommand { get; set; }
        public DelegateCommand RepeatSegmentCommand { get; set; }
        public DelegateCommand CopySegmentCommand { get; set; }
        public DelegateCommand RemoveSegmentCommand { get; set; }
        public DelegateCommand DeleteSegmentCommand { get; set; }

        public DelegateCommand MoveUpSegmentCommand { get; set; }
        public DelegateCommand MoveDownSegmentCommand { get; set; }

        #endregion DelegateCommands

        public SegmentCollectionViewModel(Func<ISegmentEditorViewModel> segmentEditorViewModelCreator)
        {
            _segmentEditorViewModelCreator = segmentEditorViewModelCreator;
            SegmentEditorVms = new ObservableCollection<ISegmentEditorViewModel>();
            selectedSegmentIndex = -1;
            SelectedSegmentEditorVm = null;

            NewSegmentCommand = new DelegateCommand(NewSegment);
            RepeatSegmentCommand = new DelegateCommand(RepeatSegment, IsOneSegmentSelected).ObservesProperty(() => SelectedSegmentIndex);
            CopySegmentCommand = new DelegateCommand(CopySegment, IsOneSegmentSelected).ObservesProperty(() => SelectedSegmentIndex);
            RemoveSegmentCommand = new DelegateCommand(RemoveSegment, IsOneSegmentSelected).ObservesProperty(() => SelectedSegmentIndex);
            DeleteSegmentCommand = new DelegateCommand(DeleteSegment, IsOneSegmentSelected).ObservesProperty(() => SelectedSegmentIndex);
            MoveUpSegmentCommand = new DelegateCommand(MoveUpSegment, CanMoveUpSegment).ObservesProperty(() => SelectedSegmentIndex);
            MoveDownSegmentCommand = new DelegateCommand(MoveDownSegment, CanMoveDownSegment).ObservesProperty(() => SelectedSegmentIndex);
        }

        private void ChangeSelectedSegmentIndex(ref int currentIndex, int value, [CallerMemberName] string propertyName = null)
        {
            if (SetProperty(ref currentIndex, value, propertyName))
            {
                SelectedSegmentEditorVm = SegmentEditorVms[currentIndex];
            }
        }

        private void NewSegment()
        {
            var segmentEditorViewModel = _segmentEditorViewModelCreator();
            SegmentEditorVms.Add(segmentEditorViewModel);
            SelectedSegmentIndex = SegmentEditorVms.Count - 1;
        }

        private void RepeatSegment()
        {
            SegmentEditorVms.Add(SelectedSegmentEditorVm);
            SelectedSegmentIndex = SegmentEditorVms.Count - 1;
        }

        private void CopySegment()
        {
            var copy = SelectedSegmentEditorVm.DeepCopy();
            copy.SegmentName = SelectedSegmentEditorVm.SegmentName + "Copied";
            SegmentEditorVms.Add(copy);
            SelectedSegmentIndex = SegmentEditorVms.Count - 1;
        }

        private void RemoveSegment()
        {
        }

        private void DeleteSegment()
        {
        }

        private void MoveUpSegment()
        {
            int newSelectedIndex = SelectedSegmentIndex - 1;
            SegmentEditorVms.Move(SelectedSegmentIndex, newSelectedIndex);
            SelectedSegmentIndex = newSelectedIndex;
        }


        private bool CanMoveUpSegment()
        {
            if (!IsOneSegmentSelected()) return false;
            return !IsSelectedSegmentFirst();
        }

        private bool IsSelectedSegmentFirst()
        {
            return SelectedSegmentIndex == 0;
        }

        private void MoveDownSegment()
        {
            int newSelectedIndex = SelectedSegmentIndex + 1;
            SegmentEditorVms.Move(SelectedSegmentIndex, newSelectedIndex);
            SelectedSegmentIndex = newSelectedIndex;
        }

        private bool CanMoveDownSegment()
        {
            if (!IsOneSegmentSelected()) return false;
            return !IsSelectedSegmentLast();
        }

        private bool IsSelectedSegmentLast()
        {
            return SelectedSegmentIndex == SegmentEditorVms.Count - 1;
        }

        private bool IsOneSegmentSelected()
        {
            return SelectedSegmentEditorVm != null;
        }
    }
}