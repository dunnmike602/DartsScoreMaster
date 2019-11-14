using System.Threading.Tasks;
using Windows.Storage;

namespace DartsScoreMaster.Repositories.Serialization.Interfaces
{
    public interface ISerialization<T> where T : class
    {
        Task<int> Serialize(T instance);

        Task<T> Deserialize();

        string FileName { get; set; }

        StorageFolder Folder { get; set; }

        Task Restore(string folderName);
    }
}