using Windows.UI.Popups;

namespace DartsScoreMaster.Services.Interfaces
{
    public interface IDialogService
    {
        void ShowMessage(string message, string title, UICommandInvokedHandler yesCommandHandler);
    }
}