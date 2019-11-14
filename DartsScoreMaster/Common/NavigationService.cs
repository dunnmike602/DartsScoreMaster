using System;
using Windows.UI.Xaml.Controls;

namespace DartsScoreMaster.Common
{
    public class NavigationService : INavigationService
    {
        public Frame Frame  { get; set; }

        public NavigationService(Frame frame)
        {
            Frame = frame;
        }

        public void GoBack()
        {
            Frame.GoBack();
        }

        public void GoForward()
        {
            Frame.GoForward();
        }

        public bool Navigate<T>(object parameter = null)
        {
            var type = typeof(T);

            return Navigate(type, parameter);
        }

        public bool Navigate(Type source, object parameter = null)
        {
            return Frame.Navigate(source, parameter);
        }
    }

}
