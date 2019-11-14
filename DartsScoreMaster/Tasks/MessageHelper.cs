using System;
using Windows.UI.Popups;

namespace DartsScoreMaster.Tasks
{
    public static class MessageHelper
    {
        public static async void ShowMessage(string message)
        {
            var messageDialog = new MessageDialog(message);
            await messageDialog.ShowAsync();
        }
    }
}