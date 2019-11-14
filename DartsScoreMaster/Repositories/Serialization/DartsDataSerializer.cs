using System;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.Storage;
using DartsScoreMaster.Repositories.Serialization.Interfaces;

namespace DartsScoreMaster.Repositories.Serialization
{
    public class DartsDataSerializer<T> : ISerialization<T> where T : class
    {
        public async Task Restore(string folderName)
        {
            var folder = (StorageFolder)await ApplicationData.Current.LocalFolder.TryGetItemAsync(folderName);

            if (folder != null)
            {
               await ApplicationData.Current.LocalFolder.DeleteAllConfigFiles(false);

               await folder.CopyFolder(ApplicationData.Current.LocalFolder);
            }
        }

        public async Task<int> Serialize(T instance)
        {
            var serializer = new DataContractSerializer(typeof(T));

            string content;
        
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, instance);
                stream.Position = 0;
                content = new StreamReader(stream).ReadToEnd();
            }
            
            await BackupFiles();

            var file = await Folder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, content);

            return content.Length/1024;
        }

        private async Task BackupFiles()
        {
            await DartsDataSerializerHelper.BackupAllFiles(Folder);
        }

        public async Task<T> Deserialize()
        {
            var file = await Folder.TryGetItemAsync(FileName).AsTask().ConfigureAwait(false);
       
            if (file != null)
            {
                using (var inputStream = await ((StorageFile) file).OpenReadAsync().AsTask().ConfigureAwait(false))
                {
                    var serializer = new DataContractSerializer(typeof (T));

                    return (T) serializer.ReadObject(inputStream.AsStreamForRead());
                }
            }

            return null;
        }
        
        public string FileName { get; set; }

        public StorageFolder Folder { get; set; }
    }
}
