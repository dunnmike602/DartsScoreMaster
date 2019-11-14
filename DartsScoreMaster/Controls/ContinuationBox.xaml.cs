using System;
using Windows.UI.Xaml;
using ReactiveUI;

namespace DartsScoreMaster.Controls
{
    public sealed partial class ContinuationBox
    {
         public ReactiveCommand<object> FadeCommand
        {
            get { return (ReactiveCommand<object>)GetValue(FadeCommandProperty); }
            set
            {
                SetValue(FadeCommandProperty, value);
            }
        }

        private void FadeCommandHandler(object fadeIn)
        {
            if ((bool) fadeIn)
            {
                ShowButtons();
            }
            else
            {
                HideButtons();
            }
        }

        public static readonly DependencyProperty FadeCommandProperty =
            DependencyProperty.Register("FadeCommand", typeof(ReactiveCommand<object>), typeof(ContinuationBox),
                new PropertyMetadata(null, FadeChangedEventHandler));

        private static void FadeChangedEventHandler(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var box = source as ContinuationBox;

            if (box != null)
            {
                box.SetUpFadeCommand();
            }
        }

        private void SetUpFadeCommand()
        {
            FadeCommand.Subscribe(FadeCommandHandler);
        }

        private void HideButtons()
        {
            LayoutRoot.Opacity = 0;
            LayoutRoot.Visibility = Visibility.Collapsed;
        }

        private void ShowButtons()
        {
           LayoutRoot.Visibility = Visibility.Visible;
           FadeIn.Begin();
        }

        public ContinuationBox()
        {
            InitializeComponent();
        }
    }
}
