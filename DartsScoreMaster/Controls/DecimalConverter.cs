using System;
using Windows.UI.Xaml.Data;

namespace DartsScoreMaster.Controls
{
    public class DecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value.ToString() == "0" ? null : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}