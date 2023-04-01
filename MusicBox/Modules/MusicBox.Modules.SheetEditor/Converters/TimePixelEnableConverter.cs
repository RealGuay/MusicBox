using MusicBox.Modules.SheetEditor.Models;
using MusicBox.Services.Interfaces.MusicSheetModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace MusicBox.Modules.SheetEditor.Converters
{
    internal class TimePixelEnableConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length != 2) throw new ArgumentException("Invalid number of values");
            PlayingHand selectedHand = (PlayingHand) values[0];
            TimePixel timepixel = (TimePixel) values[1];
            //if (timepixel.Status >= TimePixelStatus.PixelOn)
            //{
            //    return selectedHand == timepixel.Hand;
            //}
            return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
