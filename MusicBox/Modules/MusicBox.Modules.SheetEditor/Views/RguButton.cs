using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace MusicBox.Modules.SheetEditor.Views
{
    class RguButton : Button
    {
       static private int count = 0;

        public RguButton()
        {
            count++;
        }

        ~RguButton()
        {
            count--;
        }

    }
}
