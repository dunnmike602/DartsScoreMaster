using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace DartsScoreMaster.Repositories.Serialization
{
    public class DartsDataSerializerHelper
    {
        public const string ErrorFileStem = "Error";
        public const string BackupFolderStem = "BACKUP";
        public const string DateFormat = "yyyyMMdd";
        public const string LongDateFormat = "dd-MMM-yyyy";
        
        public static async Task BackupAllFiles(StorageFolder folder)
        {
            var destinationFolder = await DeleteAllBackupFiles();

            await folder.CopyFolder(destinationFolder);
        }

        public static async Task PruneBackupDirectories()
        {
            var folders = await ApplicationData.Current.LocalFolder.GetFoldersAsync();

            foreach (var folder in folders)
            {
                if (folder.Name.Length > 8 && folder.Name.StartsWith(BackupFolderStem))
                {
                    var date = folder.Name.Substring(folder.Name.Length - 8);

                    if (DateTime.Today.Subtract(DateTime.ParseExact(date, DateFormat, CultureInfo.InvariantCulture)).Days > 4)
                    {
                        await folder.DeleteAsync(StorageDeleteOption.PermanentDelete);
                    }
                }
            }
        }

        public static async Task PruneErrors()
        {
            var folder = ApplicationData.Current.LocalFolder;

            foreach (var file in await folder.GetFilesAsync())
            {
                var nameOnly = Path.GetFileNameWithoutExtension(file.Name);

                if (nameOnly.Length > 8 && nameOnly.StartsWith(ErrorFileStem))
                {
                    var date = nameOnly.Substring(nameOnly.Length - 8);

                    if (DateTime.Today.Subtract(DateTime.ParseExact(date, DateFormat, CultureInfo.InvariantCulture)).Days >
                        8)
                    {
                        await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
                    }
                }
            }
        }

        public static async Task<string> ReadStringFromLocalFile(string filename)
        {
            // reads the contents of file 'filename' in the app's local storage folder and returns it as a string

            // access the local folder
            var local = ApplicationData.Current.LocalFolder;

            // open the file 'filename' for reading
            var stream = await local.OpenStreamForReadAsync(filename);
            string text;

            // copy the file contents into the string 'text'
            using (var reader = new StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }

            return text;
        }


        public static async Task<string> GetErrorFilesAsText()
        {
            var files = await ApplicationData.Current.LocalFolder.GetFilesAsync();

            var content = string.Empty;

            foreach (var file in files)
            {
                if (file.Name.StartsWith(ErrorFileStem) && file.Name.Length > 8)
                {
                    content += await ReadStringFromLocalFile(file.Name);
                }
            }

            return content;
        }

        public static async Task<string> DeleteAllErrorFiles()
        {
            var files = await ApplicationData.Current.LocalFolder.GetFilesAsync();

            var content = string.Empty;

            foreach (var file in files)
            {
                await file.DeleteAsync();
            }

            return content;
        }
        public static async Task<bool> HasErrors()
        {
            var files = await ApplicationData.Current.LocalFolder.GetFilesAsync();

            foreach (var file in files)
            {
                if (file.Name.StartsWith(ErrorFileStem) && file.Name.Length > 8)
                {
                    return true;
                }
            }

            return false;
        }

        public static async Task<List<string>> GetAllBackups()
        {
            var folders = await ApplicationData.Current.LocalFolder.GetFoldersAsync();

            var backups = new List<DateTime>();

            foreach (var folder in folders)
            {
                if (folder.Name.StartsWith(BackupFolderStem) && folder.Name.Length > 8)
                {
                    var dateText = folder.Name.Substring(folder.Name.Length - 8);

                    backups.Add(DateTime.ParseExact(dateText, DateFormat, CultureInfo.InvariantCulture));
                }
            }

            return backups.OrderByDescending(m => m).Select(m => m.ToString(DartsDataSerializerHelper.LongDateFormat)).ToList();
        }

        public static string GetBackupFolder(string date)
        {
            return BackupFolderStem + date;
        }

        public static string GetErrorFileName(string date)
        {
            return ErrorFileStem + date + ".txt";
        }

        public static async Task<StorageFolder> DeleteAllBackupFiles()
        {
            var backupFolder = GetBackupFolder(DateTime.Today.ToString(DateFormat));

            var folder = (StorageFolder)await ApplicationData.Current.LocalFolder.TryGetItemAsync(backupFolder);

            if (folder != null)
            {
                await folder.DeleteAllConfigFiles(false);

                return folder;
            }

            return await ApplicationData.Current.LocalFolder.CreateFolderAsync(backupFolder);
        }
    }
}