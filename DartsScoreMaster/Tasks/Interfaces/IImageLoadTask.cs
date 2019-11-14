using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using DartsScoreMaster.Model;

namespace DartsScoreMaster.Tasks.Interfaces
{
    public interface IImageLoadTask
    {
        Task<Tuple<BitmapImage, byte[]>> ConvertAndResize(StorageFile sourceFile, int width, int height);
        Task<ImageDefinition> PickThumbnailImage();
        Task<BitmapImage> LoadImageFromContent(string fileName);
        Task<BitmapImage> LoadImage(StorageFile file);
        Task<BitmapImage> LoadThumbnailImageFromContent(string fileName);
        Task<ImageDefinition> GetImageDefinitionFromFile(StorageFile sourceFile);
    }
}