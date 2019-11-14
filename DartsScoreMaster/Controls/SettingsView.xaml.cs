using DartsScoreMaster.ViewModels;

namespace DartsScoreMaster.Controls
{
    public sealed partial class SettingsView
    {
        public SettingsView()
        {
            InitializeComponent();

            DataContext = MainLocator.SettingsViewModel;
        }
    }
}
