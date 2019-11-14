using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml.Controls;
using DartsScoreMaster.Controls;
using DartsScoreMaster.ViewModels;

namespace DartsScoreMaster.Settings
{
    internal static class Settings
    {
        public static void Initialise()
        {
            SettingsPane settingsPane = SettingsPane.GetForCurrentView();

            settingsPane.CommandsRequested += (s, e) =>
            {
                var settingsCommand = new SettingsCommand(
                  "SETTINGS_ID",
                  "Settings", command =>
                  {
                      var view = new SettingsView();

                      var flyout = new SettingsFlyout
                      {
                          Title = "Settings",
                          Content = view
                      };

                      ((SettingsViewModel) (view.DataContext)).Init();

                      flyout.Show();
                  }
                );
                e.Request.ApplicationCommands.Add(settingsCommand);
            };
        }
    }


}
