using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using DartsScoreMaster.Model;
using DartsScoreMaster.Repositories.Interfaces;
using DartsScoreMaster.Repositories.Serialization.Interfaces;

namespace DartsScoreMaster.Repositories
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        private readonly ISerialization<GameConfiguration> _configSerialiser;
        public const string ConfigFileName = "CONFIG.XML";

        public static GameConfiguration ConfigDetailsCache { get; private set; }

        public ConfigurationRepository(ISerialization<GameConfiguration> configSerialiser)
        {
            _configSerialiser = configSerialiser;

            _configSerialiser.FileName = ConfigFileName;
            _configSerialiser.Folder = ApplicationData.Current.LocalFolder;
        }

        public void Add(GameConfiguration playerDetails)
        {
            ConfigDetailsCache = playerDetails;
        }

        public async Task<GameConfiguration> GetAll()
        {

            try
            {
                var fileObject = await _configSerialiser.Deserialize();
                ConfigDetailsCache = fileObject ?? await EnsureCache();
            }
            catch (AggregateException ex)
            {
                
                // If there is a problem opening the settings just discard and create a new one
                if (ex.InnerExceptions[0].Message == "Unexpected end of file.")
                {
                    ConfigDetailsCache = await EnsureCache();
                }
                else
                {
                    throw;
                }
            }
        
            return ConfigDetailsCache;
        }

        private async Task<GameConfiguration> EnsureCache()
        {
            if (ConfigDetailsCache == null)
            {
                ConfigDetailsCache = new GameConfiguration{PlaySounds = true, ShowCheckoutHints = true,
                GameType = GameType.T501,LegsPerSet = 1, PlayerIndexes = new List<Guid>(),Sets=1,
                PlayerIds = new List<int>()};
                await Save();
            }

            return ConfigDetailsCache;
        }

        public async Task Save()
        {
           await _configSerialiser.Serialize(ConfigDetailsCache);
        }

        public async Task Restore(string folderName)
        {
            await _configSerialiser.Restore(folderName);
        }
    }
}