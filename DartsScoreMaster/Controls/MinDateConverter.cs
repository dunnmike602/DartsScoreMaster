using System;
using Windows.UI.Xaml.Data;

namespace DartsScoreMaster.Controls
{
    public class MinDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var actualDate = (string) value;

            if (actualDate == "01-Jan-0001")
            {
                return null;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}