using MusicBox.Modules.SheetEditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Prism.Events;

namespace MusicBox.Modules.SheetEditor.Models
{
    public class SelectedBarChanged : PubSubEvent<IBarEditorViewModel>
    {
    }
}
