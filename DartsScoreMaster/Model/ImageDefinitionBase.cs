using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using DartsScoreMaster.ViewModels;

namespace DartsScoreMaster.Model
{
    public abstract class ImageDefinitionBase : BaseViewModel
    {
        private string _name;
        private BitmapImage _source;
        private byte[] _sourceBytes;

        public string Name
        {
            get { return _name; }
            set
            {
                Set(() => Name, ref _name, value);
            }
        }
        
        public BitmapImage Source
        {
            get { return _source; }
            set
            {
                Set(() => Source, ref _source, value);
            }
        }

        public byte[] SourceBytes
        {
            get { return _sourceBytes;}
            set
            {
                Set(() => SourceBytes, ref _sourceBytes, value);
            }
        }

       
    }
}