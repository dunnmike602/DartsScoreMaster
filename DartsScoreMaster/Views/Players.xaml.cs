using DartsScoreMaster.ViewModels;

namespace DartsScoreMaster.Views
{
    public sealed partial class Players
    {
        public Players()
        {
            InitializeComponent();

            DataContext = MainLocator.PlayerViewModel;
        }
    }
}
