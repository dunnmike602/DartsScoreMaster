using Windows.UI.Xaml.Media.Imaging;
using GalaSoft.MvvmLight;

namespace DartsScoreMaster.Model
{
    public class Hint : ObservableObject
    {
        private string _hintText;
        private BitmapImage _hintImage;

        public string HintText
        {
            get { return _hintText; }
            set
            {
                Set(() => HintText, ref _hintText, value);
            }
        }

        public BitmapImage HintImage
        {
            get { return _hintImage; }
            set
            {
                Set(() => HintImage, ref _hintImage, value);
            }
        }
    }
}