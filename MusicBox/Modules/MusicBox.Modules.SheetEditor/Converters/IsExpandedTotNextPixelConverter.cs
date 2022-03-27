using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace MusicBox.Modules.SheetEditor.Converters
{
    public class IsExpandedTotNextPixelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool IsExpanded = (bool)value;
            if (IsExpanded)
            {
                return Brushes.Orange;
            }
            return Brushes.White;


            //if (status == TimePixelStatus.OnStaffLine) return "#FF7C7E7E";
            //if (status == TimePixelStatus.PixelOn) return Brushes.Black;
            //if (status == TimePixelStatus.PixelOnAndFlat) return Brushes.LightGreen;
            //if (status == TimePixelStatus.PixelOnAndSharp) return Brushes.LightSalmon;
            //return Brushes.WhiteSmoke;


        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
