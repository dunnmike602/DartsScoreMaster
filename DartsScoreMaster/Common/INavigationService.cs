using System;
using Windows.UI.Xaml.Controls;

namespace DartsScoreMaster.Common
{
    public interface INavigationService
    {
        void GoBack();
        void GoForward();
        bool Navigate<T>(object parameter = null);
        bool Navigate(Type source, object parameter = null);
        Frame Frame { get; set; }
    }
}