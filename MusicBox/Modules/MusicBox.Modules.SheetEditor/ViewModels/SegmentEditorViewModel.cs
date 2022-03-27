using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;

namespace MusicBox.Modules.SheetEditor.ViewModels
{
    public class SegmentEditorViewModel : BindableBase, ISegmentEditorViewModel
    {
        private const string DefaultName = "Segment1";

        private readonly Func<IBarEditorViewModel> _barEditorVmCreator;
        private string segmentName;

        public string SegmentName { get => segmentName; set => SetProperty(ref segmentName, value); }

        private ObservableCollection<IBarEditorViewModel> barEditorVms;
        public ObservableCollection<IBarEditorViewModel> BarEditorVms { get => barEditorVms; set => SetProperty(ref barEditorVms, value); }

        private IBarEditorViewModel selectedBarEditorVm;
        public IBarEditorViewModel SelectedBarEditorVm { get => selectedBarEditorVm; set => SetProperty(ref selectedBarEditorVm, value); }

        public DelegateCommand AddBarCommand { get; set; }
        public DelegateCommand DeleteBarCommand { get; set; }

        public SegmentEditorViewModel(Func<IBarEditorViewModel> barEditorVmCreator)
        {
            _barEditorVmCreator = barEditorVmCreator;
            SegmentName = DefaultName;
            BarEditorVms = new ObservableCollection<IBarEditorViewModel>();
            AddBarCommand = new DelegateCommand(AddBar);
            DeleteBarCommand = new DelegateCommand(DeleteBar, CanDeleteBar).ObservesProperty(() => SelectedBarEditorVm);
        }

        private void AddBar()
        {
            var newBarEditorVm = _barEditorVmCreator();
            BarEditorVms.Add(newBarEditorVm);
            SelectedBarEditorVm = newBarEditorVm;
        }

        private void DeleteBar()
        {
            int removedIndex = BarEditorVms.IndexOf(SelectedBarEditorVm);

            BarEditorVms.RemoveAt(removedIndex);

            int newSelectedIndex = removedIndex < BarEditorVms.Count ? removedIndex : removedIndex - 1;
            if (newSelectedIndex > -1)
            {
                SelectedBarEditorVm = BarEditorVms[newSelectedIndex];
            }
            DeleteBarCommand?.RaiseCanExecuteChanged();
        }

        private bool CanDeleteBar()
        {
            return SelectedBarEditorVm != null;
        }

        public ISegmentEditorViewModel DeepCopy()
        {
            SegmentEditorViewModel copy = new SegmentEditorViewModel(_barEditorVmCreator)
            {
                SegmentName = SegmentName
            };

            foreach (BarEditorViewModel barEditorViewModel in BarEditorVms)
            {
                IBarEditorViewModel newBarEditorVm = barEditorViewModel.DeepCopy();
                copy.BarEditorVms.Add(newBarEditorVm);
            }
            return copy;
        }
    }
}