using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;

namespace MusicBox.Modules.SheetEditor.ViewModels
{
    public class SegmentCollectionViewModel : BindableBase
    {
        #region Properties

        private ObservableCollection<ISegmentEditorViewModel> segmentEditorVms;

        public ObservableCollection<ISegmentEditorViewModel> SegmentEditorVms { get => segmentEditorVms; set => SetProperty(ref segmentEditorVms, value); }

        private ISegmentEditorViewModel selectedSegmentEditorVm;

        public ISegmentEditorViewModel SelectedSegmentEditorVm { get => selectedSegmentEditorVm; set => SetProperty(ref selectedSegmentEditorVm, value); }

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

            NewSegmentCommand = new DelegateCommand(NewSegment);
            RepeatSegmentCommand = new DelegateCommand(RepeatSegment, IsOneSegmentSelected).ObservesProperty(() => SelectedSegmentEditorVm);
            CopySegmentCommand = new DelegateCommand(CopySegment, IsOneSegmentSelected).ObservesProperty(() => SelectedSegmentEditorVm); ;
            RemoveSegmentCommand = new DelegateCommand(RemoveSegment, IsOneSegmentSelected).ObservesProperty(() => SelectedSegmentEditorVm); ;
            DeleteSegmentCommand = new DelegateCommand(DeleteSegment, IsOneSegmentSelected).ObservesProperty(() => SelectedSegmentEditorVm); ;
            MoveUpSegmentCommand = new DelegateCommand(MoveUpSegment, CanMoveUpSegment).ObservesProperty(() => SelectedSegmentEditorVm); ;
            MoveDownSegmentCommand = new DelegateCommand(MoveDownSegment, CanMoveDownSegment).ObservesProperty(() => SelectedSegmentEditorVm); ;
        }

        private void NewSegment()
        {
            var segmentEditorViewModel = _segmentEditorViewModelCreator();
            SegmentEditorVms.Add(segmentEditorViewModel);
            SelectedSegmentEditorVm = segmentEditorViewModel;
        }

        private void RepeatSegment()
        {
            var repeat = SelectedSegmentEditorVm.DeepCopy();
            repeat.SegmentName = SelectedSegmentEditorVm.SegmentName + "Repeated";
            SegmentEditorVms.Add(repeat);
            SelectedSegmentEditorVm = repeat;
        }

        private void CopySegment()
        {
        }

        private void RemoveSegment()
        {
        }

        private void DeleteSegment()
        {
        }

        private void MoveUpSegment()
        {
            int index = SegmentEditorVms.IndexOf(SelectedSegmentEditorVm);
            if (index > 0)
            {
                SegmentEditorVms.Move(index, index - 1);
                RefreshUpDownButtons();
            }
        }

        private void RefreshUpDownButtons()
        {
            MoveUpSegmentCommand.RaiseCanExecuteChanged();
            MoveDownSegmentCommand.RaiseCanExecuteChanged();
        }

        private bool CanMoveUpSegment()
        {
            if (!IsOneSegmentSelected()) return false;
            return !IsSelectedSegmentFirst();
        }

        private bool IsSelectedSegmentFirst()
        {
            return SegmentEditorVms.IndexOf(SelectedSegmentEditorVm) == 0;
        }

        private void MoveDownSegment()
        {
            int index = SegmentEditorVms.IndexOf(SelectedSegmentEditorVm);
            if (index < SegmentEditorVms.Count - 1)
            {
                SegmentEditorVms.Move(index, index + 1);
                RefreshUpDownButtons();
            }
        }

        private bool CanMoveDownSegment()
        {
            if (!IsOneSegmentSelected()) return false;
            return !IsSelectedSegmentLast();
        }

        private bool IsSelectedSegmentLast()
        {
            return SegmentEditorVms.IndexOf(SelectedSegmentEditorVm) == SegmentEditorVms.Count - 1;
        }

        private bool IsOneSegmentSelected()
        {
            return SelectedSegmentEditorVm != null;
        }
    }
}