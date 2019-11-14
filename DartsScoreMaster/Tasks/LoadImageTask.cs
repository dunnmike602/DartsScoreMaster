using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using DartsScoreMaster.Model;
using DartsScoreMaster.Tasks.Interfaces;

namespace DartsScoreMaster.Tasks
{
    public class ImageLoadTask : IImageLoadTask
    {
        public async Task<BitmapImage> LoadImageFromContent(string fileName)
        {
            var folder = Package.Current.InstalledLocation;

            var fileLocation = @"Content\" + fileName;

            var file = await folder.GetFileAsync(fileLocation);

            var img = new BitmapImage();

            using (var fileStream = await file.OpenReadAsync())
            {
                img.SetSource(fileStream);
            }

            return img;
        }

        public async Task<BitmapImage> LoadThumbnailImageFromContent(string fileName)
        {
            var folder = Package.Current.InstalledLocation;

            var fileLocation = @"Content\" + fileName;

            var file = await folder.GetFileAsync(fileLocation);

            var img = new BitmapImage();

            using (var fileStream = await file.GetScaledImageAsThumbnailAsync(ThumbnailMode.PicturesView, 100))
            {
                img.SetSource(fileStream);
            }

            return img;
        }

        public async Task<BitmapImage> LoadImage(StorageFile file)
        {
            var img = new BitmapImage();

            using (var fileStream = await file.OpenReadAsync())
            {
                img.SetSource(fileStream);
            }

            return img;
        }

        public async Task<ImageDefinition> GetImageDefinitionFromFile(StorageFile sourceFile)
        {
            if (sourceFile == null)
            {
                return null;
            }

            var imageData = await ConvertAndResize(sourceFile, 200, 200);

            return new ImageDefinition { Source = imageData.Item1, SourceBytes = imageData.Item2 };
        }

        public async Task<Tuple<BitmapImage, byte[]>> ConvertAndResize(StorageFile sourceFile, int width, int height)
        {
            WriteableBitmap wb;

            using (var sourceStream = await sourceFile.OpenAsync(FileAccessMode.Read))
            {
                wb = await BitmapFactory.FromStream(sourceStream);
            }
            
            var newBitmap = wb.Resize(width, height, WriteableBitmapExtensions.Interpolation.Bilinear);

            var memStream = new InMemoryRandomAccessStream();

            await newBitmap.ToStream(memStream, Windows.Graphics.Imaging.BitmapEncoder.JpegEncoderId);

            using (IRandomAccessStream fileStream = memStream)
            {
                var reader = new DataReader(fileStream.GetInputStreamAt(0));
                var bytes = new byte[fileStream.Size];
                await reader.LoadAsync((uint) fileStream.Size);
                reader.ReadBytes(bytes);

                var image = new BitmapImage();
                image.SetSource(memStream);

                return new Tuple<BitmapImage, byte[]>(image, bytes);
            }
        }

        public async Task<ImageDefinition> PickThumbnailImage()
        {
            try
            {
                var picker = new FileOpenPicker
                {
                    ViewMode = PickerViewMode.List,
                    SuggestedStartLocation = PickerLocationId.PicturesLibrary
                };

                picker.FileTypeFilter.Add(".jpeg");
                picker.FileTypeFilter.Add(".jpg");
                picker.FileTypeFilter.Add(".png");
                picker.FileTypeFilter.Add(".gif");
                picker.FileTypeFilter.Add(".tiff");
                picker.FileTypeFilter.Add(".ico");

                var sourceFile = await picker.PickSingleFileAsync();

                return await GetImageDefinitionFromFile(sourceFile);
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage("An unexpected error occurred. The details are:" + Environment.NewLine +
                                          Environment.NewLine + ex);

                return null;
            }
        }
    }
}