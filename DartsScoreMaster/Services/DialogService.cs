using System;
using Windows.UI.Popups;
using DartsScoreMaster.Services.Interfaces;

namespace DartsScoreMaster.Services
{
    public class DialogService : IDialogService
    {
        public async void ShowMessage(string message, string title, UICommandInvokedHandler yesCommandHandler)
        {
            var messageDialog = new MessageDialog(message, title);

            messageDialog.Commands.Add(new UICommand(
                "Yes",
                yesCommandHandler));
            messageDialog.Commands.Add(new UICommand(
                "No"));

            // Set the command that will be invoked by default
            messageDialog.DefaultCommandIndex = 0;

            // Set the command to be invoked when escape is pressed
            messageDialog.CancelCommandIndex = 1;

            // Show the message dialog
            await messageDialog.ShowAsync();
        }
    }
}