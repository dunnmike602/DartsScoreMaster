using Windows.UI.Xaml.Input;

namespace DartsScoreMaster.Controls
{
    public sealed partial class StartGameButton
    {
        public StartGameButton()
        {
            InitializeComponent();
        }

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            StartButton.Flyout.Hide();
        }
    }
}
