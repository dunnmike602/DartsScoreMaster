using System;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Microsoft.Xaml.Interactivity;
using DartsScoreMaster.ViewModels.Interfaces;

namespace DartsScoreMaster.Controls
{
    public class SizeChangeBehavior : DependencyObject, IBehavior
    {
        public void Attach(DependencyObject associatedObject)
        {
            var control = associatedObject as FrameworkElement;
            if (control == null)
            {
                throw new ArgumentException("ScrollToBottomBehavior can be attached only to ItemsControl.");
            }

            AssociatedObject = associatedObject;

            ((FrameworkElement)AssociatedObject).SizeChanged += SizeChangeBehaviorSizeChanged;
        }

        private void SizeChangeBehaviorSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var orientation = ApplicationView.GetForCurrentView().Orientation;

            var fullScreen = (FrameworkElement)((FrameworkElement)AssociatedObject).FindName("FullScreen");
            var notFullScreen = (FrameworkElement)((FrameworkElement)AssociatedObject).FindName("NotFullScreen");

            if (orientation == ApplicationViewOrientation.Landscape)
            {
                fullScreen.Visibility = Visibility.Visible;
                notFullScreen.Visibility = Visibility.Collapsed;
            }
            else
            {
                fullScreen.Visibility = Visibility.Collapsed;
                notFullScreen.Visibility = Visibility.Visible;
            }

            var viewModel = (AssociatedObject as FrameworkElement).DataContext as IBaseViewModel;

            if (viewModel != null)
            {
                viewModel.UpdateSizes();
            }
        }

        public void Detach()
        {
            ((FrameworkElement)AssociatedObject).SizeChanged -= SizeChangeBehaviorSizeChanged;
            AssociatedObject = null;
        }

        public DependencyObject AssociatedObject { get; private set; }

    }
}