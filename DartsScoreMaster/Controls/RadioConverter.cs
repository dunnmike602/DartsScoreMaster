using System;
using Windows.UI.Xaml.Data;

namespace DartsScoreMaster.Controls
{
    public class RadioConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value.ToString().Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return System.Convert.ToBoolean(value) ? parameter : null;
        }
    }
}