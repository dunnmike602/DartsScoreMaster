using System.Threading.Tasks;
using DartsScoreMaster.Model;

namespace DartsScoreMaster.Repositories.Interfaces
{
    public interface IConfigurationRepository
    {
        Task<GameConfiguration> GetAll();

        void Add(GameConfiguration playerDetails);

        Task Save();

        Task Restore(string folderName);
    }
}