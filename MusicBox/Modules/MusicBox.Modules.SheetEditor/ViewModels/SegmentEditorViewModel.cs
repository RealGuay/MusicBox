﻿using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicBox.Modules.SheetEditor.ViewModels
{
    public class SegmentEditorViewModel : BindableBase
    {
        private string segmentName;

        public string SegmentName
        {
            get { return segmentName; }
            set { SetProperty(ref segmentName, value); }
        }

    }
}