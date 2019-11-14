using System;
using Windows.UI.Xaml.Data;

namespace DartsScoreMaster.Controls
{
    public class CricketConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            switch ((int)value)
            {
                default:
                    return string.Empty;

                case 1:
                    return "/";

                case 2:
                    return "X";

                case 3:
                    return "Ø";

            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}