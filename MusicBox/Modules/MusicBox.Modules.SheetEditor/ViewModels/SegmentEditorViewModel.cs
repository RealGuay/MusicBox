using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicBox.Modules.SheetEditor.ViewModels
{

    public class SegmentEditorViewModel : BindableBase, ISegmentEditorViewModel
    {
        private const string DefaultName = "Segment1";

        private string segmentName;

        public string SegmentName
        {
            get { return segmentName; }
            set { SetProperty(ref segmentName, value); }
        }

        public SegmentEditorViewModel()
        {
            SegmentName = DefaultName;
        }

        public ISegmentEditorViewModel DeepCopy()
        {
            var copy = new SegmentEditorViewModel
            {
                SegmentName = SegmentName
            };
            return copy;
        }

    }
}
