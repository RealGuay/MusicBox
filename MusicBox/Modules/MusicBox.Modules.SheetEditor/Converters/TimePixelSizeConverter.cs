using MusicBox.Modules.SheetEditor.Models;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MusicBox.Modules.SheetEditor.Converters
{
    public class TimePixelSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int pos = (int)value;
            return pos / 5;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}