using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MusicBox.Modules.SheetEditor.ViewModels
{
    public class SegmentCollectionViewModel : BindableBase
    {
        #region Properties

        private ObservableCollection<SegmentEditorViewModel> segmentEditorVms;

        public ObservableCollection<SegmentEditorViewModel> SegmentEditorVms { get => segmentEditorVms; set => SetProperty(ref segmentEditorVms, value); }

        private SegmentEditorViewModel selectedSegmentEditorVm;

        public SegmentEditorViewModel SelectedSegmentEditorVm
        {
            get { return selectedSegmentEditorVm; }
            set { SetProperty(ref selectedSegmentEditorVm, value); }
        }

        #endregion Properties

        #region ICommand

        public ICommand NewSegmentCommand { get; set; }
        public ICommand RepeatSegmentCommand { get; set; }
        public ICommand CopySegmentCommand { get; set; }
        public ICommand RemoveSegmentCommand { get; set; }
        public ICommand DeleteSegmentCommand { get; set; }

        public ICommand MoveUpSegmentCommand { get; set; }
        public ICommand MoveDownSegmentCommand { get; set; }

        #endregion ICommand

        public SegmentCollectionViewModel()
        {
            SegmentEditorVms = new ObservableCollection<SegmentEditorViewModel>();

            NewSegmentCommand = new DelegateCommand(NewSegment);
            RepeatSegmentCommand = new DelegateCommand(RepeatSegment, IsNotNullSelectedSegmentEditorVm).ObservesProperty(() => SelectedSegmentEditorVm);
            CopySegmentCommand = new DelegateCommand(CopySegment, IsNotNullSelectedSegmentEditorVm).ObservesProperty(() => SelectedSegmentEditorVm); ;
            RemoveSegmentCommand = new DelegateCommand(RemoveSegment, IsNotNullSelectedSegmentEditorVm).ObservesProperty(() => SelectedSegmentEditorVm); ;
            DeleteSegmentCommand = new DelegateCommand(DeleteSegment, IsNotNullSelectedSegmentEditorVm).ObservesProperty(() => SelectedSegmentEditorVm); ;
            MoveUpSegmentCommand = new DelegateCommand(MoveUpSegment, IsNotNullSelectedSegmentEditorVm).ObservesProperty(() => SelectedSegmentEditorVm); ;
            MoveDownSegmentCommand = new DelegateCommand(MoveDownSegment, IsNotNullSelectedSegmentEditorVm).ObservesProperty(() => SelectedSegmentEditorVm); ;

            // SIC
            var sevm = new SegmentEditorViewModel() { SegmentName = "Segment 1" };
            SegmentEditorVms.Add(sevm);
        }

        private void NewSegment()
        {
        }

        private void RepeatSegment()
        {
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
        }

        private void MoveDownSegment()
        {
        }

        private bool IsNotNullSelectedSegmentEditorVm()
        {
            return SelectedSegmentEditorVm != null;
        }
    }
}