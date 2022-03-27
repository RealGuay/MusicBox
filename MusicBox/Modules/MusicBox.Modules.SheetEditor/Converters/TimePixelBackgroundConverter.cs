using MusicBox.Modules.SheetEditor.Models;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MusicBox.Modules.SheetEditor.Converters
{
    public class TimePixelBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimePixelStatus status = (TimePixelStatus)value;
            if (status == TimePixelStatus.OnStaffLine) return "#FF7C7E7E";
            if (status == TimePixelStatus.PixelOn) return Brushes.Black;
            if (status == TimePixelStatus.PixelOnAndFlat) return Brushes.LightGreen;
            if (status == TimePixelStatus.PixelOnAndSharp) return Brushes.LightSalmon;
            return Brushes.WhiteSmoke;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}