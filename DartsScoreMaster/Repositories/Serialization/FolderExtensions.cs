using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

// ReSharper disable once CheckNamespace
namespace System
{
    public static class FolderExtensions
    {
        public static async Task CopyFolder(this StorageFolder source, StorageFolder dest)
        {
            var files = await source.GetFilesAsync();

            foreach (var file in files)
            {
                if (Path.GetExtension(file.Name).ToLower() == ".xml")
                {
                    // Do copy file to destination folder
                    await file.CopyAsync(dest);
                }

            }
        }

        public static async Task DeleteAllConfigFiles(this StorageFolder source, bool ignoreConfigFile)
        {
            var items = await source.GetFilesAsync();

            foreach (var file in items.Where(file => file != null && Path.GetExtension(file.Name).ToLower() == ".xml"))
            {
                if (ignoreConfigFile && file.Name.ToLower() == "config.xml")
                {
                    continue;
                }

                await file.DeleteAsync();
            }
        }
    }
}