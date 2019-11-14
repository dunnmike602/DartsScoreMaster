using DartsScoreMaster.ViewModels;

namespace DartsScoreMaster.Views
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class Hub
    {
        public Hub()
        {
            InitializeComponent();

            DataContext = MainLocator.HubViewModel;
        }

    }
}
