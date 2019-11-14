using System;
using Windows.UI.Xaml.Data;

namespace DartsScoreMaster.Controls
{
    public class FontConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool)value ? (double)30 : (double)25;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}