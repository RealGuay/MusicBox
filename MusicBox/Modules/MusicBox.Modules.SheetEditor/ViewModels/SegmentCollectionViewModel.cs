using MusicBox.Services.Interfaces.MusicSheetModels;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private readonly IDialogService _dialogService;

        #region DelegateCommands

        public DelegateCommand NewSegmentCommand { get; set; }
        public DelegateCommand RepeatSegmentCommand { get; set; }
        public DelegateCommand CopySegmentCommand { get; set; }
        public DelegateCommand DeleteSegmentCommand { get; set; }

        public DelegateCommand MoveUpSegmentCommand { get; set; }
        public DelegateCommand MoveDownSegmentCommand { get; set; }

        #endregion DelegateCommands

        public SegmentCollectionViewModel(Func<ISegmentEditorViewModel> segmentEditorViewModelCreator, IDialogService dialogService)
        {
            _segmentEditorViewModelCreator = segmentEditorViewModelCreator;
            _dialogService = dialogService;
            SegmentEditorVms = new ObservableCollection<ISegmentEditorViewModel>();
            SelectedSegmentIndex = -1;
            SelectedSegmentEditorVm = null;

            NewSegmentCommand = new DelegateCommand(NewSegment);
            RepeatSegmentCommand = new DelegateCommand(RepeatSegment, IsOneSegmentSelected).ObservesProperty(() => SelectedSegmentIndex);
            CopySegmentCommand = new DelegateCommand(CopySegment, IsOneSegmentSelected).ObservesProperty(() => SelectedSegmentIndex);
            DeleteSegmentCommand = new DelegateCommand(DeleteSegment, IsOneSegmentSelected).ObservesProperty(() => SelectedSegmentIndex);
            MoveUpSegmentCommand = new DelegateCommand(MoveUpSegment, CanMoveUpSegment).ObservesProperty(() => SelectedSegmentIndex);
            MoveDownSegmentCommand = new DelegateCommand(MoveDownSegment, CanMoveDownSegment).ObservesProperty(() => SelectedSegmentIndex);
        }

        private void ChangeSelectedSegmentIndex(ref int currentIndex, int value, [CallerMemberName] string propertyName = null)
        {
            if (SetProperty(ref currentIndex, value, propertyName))
            {
                if (currentIndex > -1)
                {
                    SelectedSegmentEditorVm = SegmentEditorVms[currentIndex];
                }
                else
                {
                    SelectedSegmentEditorVm = null;
                }
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

        public void LoadSegments(List<Segment> segments)
        {
            ISegmentEditorViewModel segmentEditorVm;
            Dictionary<string, ISegmentEditorViewModel> distinctSegments = new Dictionary<string, ISegmentEditorViewModel>();

            SegmentEditorVms.Clear();
            foreach (var segment in segments)
            {
                if (!distinctSegments.ContainsKey(segment.Name))
                {
                    segmentEditorVm = _segmentEditorViewModelCreator();
                    segmentEditorVm.LoadSegmentInfo(segment);
                    distinctSegments.Add(segment.Name, segmentEditorVm);
                }
                else
                {
                    segmentEditorVm = distinctSegments[segment.Name];
                }
                SegmentEditorVms.Add(segmentEditorVm);
            }
        }

        private void DeleteSegment()
        {
            IDialogResult result = null;

            if (segmentEditorVms.Count(s => s == SelectedSegmentEditorVm) > 1)
            {
                result = new DialogResult(ButtonResult.Yes);
            }
            else
            {
                DialogParameters parameters = new DialogParameters();
                parameters.Add("title", "Delete Confirmation");
                parameters.Add("message", "Do you really want to delete this segment ?");
                _dialogService.ShowDialog("MessageDialog", parameters, (r) => result = r);
            }

            if (result.Result == ButtonResult.Yes)
            {
                DoDeleteSegment();
            }
        }

        private void DoDeleteSegment()
        {
            int removedIndex = SelectedSegmentIndex;
            SegmentEditorVms.RemoveAt(removedIndex);
            SelectedSegmentIndex = removedIndex < SegmentEditorVms.Count ? removedIndex : removedIndex - 1;
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