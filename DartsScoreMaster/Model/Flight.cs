using System.Runtime.Serialization;
using Windows.UI.Xaml.Media.Imaging;

namespace DartsScoreMaster.Model
{
    [DataContract]
    public class Flight
    {
        [DataMember]
        public int Index { get; set; }

        public BitmapImage Image { get; set; }
    }
}