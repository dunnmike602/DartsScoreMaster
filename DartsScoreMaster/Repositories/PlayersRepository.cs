using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;
using DartsScoreMaster.Model;
using DartsScoreMaster.Repositories.Interfaces;
using DartsScoreMaster.Repositories.Serialization.Interfaces;

namespace DartsScoreMaster.Repositories
{
    public class PlayersRepository : IPlayersRepository
    {
        private readonly ISerialization<List<PlayerDetailsDto>> _playerSerialiser;
        public const string PlayersFileName = "PLAYERSLIST.XML";

        public PlayersRepository(ISerialization<List<PlayerDetailsDto>> playerSerialiser )
        {
            _playerSerialiser = playerSerialiser;

            _playerSerialiser.FileName = PlayersFileName;
            _playerSerialiser.Folder = ApplicationData.Current.LocalFolder;
        }

        public static List<PlayerDetails> PlayerDetailsCache { get; private set; }

        public void DeleteById(Guid playerDetailsId)
        {
            EnsureCache();

            var currentItem = PlayerDetailsCache.FirstOrDefault(m => m.Id == playerDetailsId);

            if (currentItem != null)
            {
                PlayerDetailsCache.Remove(currentItem);
            }
        }

        public void Add(PlayerDetails playerDetails)
        {
            EnsureCache();

            var currentItem = PlayerDetailsCache.FirstOrDefault(m => m.Id == playerDetails.Id);

             if (currentItem == null)
             {
                 PlayerDetailsCache.Add(playerDetails);
             }
             else
             {
                 currentItem.CopyFrom(playerDetails);
             }
        }

        public async void SaveAll()
        {
           var dataToSave = PlayerDetailsCache.Select(playerDetails =>
                {
                    var details = new PlayerDetailsDto
                    {
                        Handicap101 = playerDetails.Handicap101,
                        Handicap201 = playerDetails.Handicap201,
                        Handicap301 = playerDetails.Handicap301,
                        Handicap401 = playerDetails.Handicap401,
                        Handicap501 = playerDetails.Handicap501,
                        HandicapCricket = playerDetails.HandicapCricket,
                        Name = playerDetails.Name,
                        NickName = playerDetails.NickName,
                        Id = playerDetails.Id
                    };
                    var definition = new ImageDefinitionDto
                    {
                        Name = playerDetails.PlayerImageDefinition.Name,
                        SourceBytes = playerDetails.PlayerImageDefinition.SourceBytes
                    };
                    details.PlayerImageDefinition = definition;
                    details.SelectedFlight = playerDetails.SelectedFlight;
                    return details;
                }).ToList();

           await _playerSerialiser.Serialize(dataToSave);
        }
        
        public  List<PlayerDetails> GetAll()
        {
            var fileObject =  _playerSerialiser.Deserialize();

            if (fileObject.Result == null)
            {
                EnsureCache();
            }
            else
            {
                PlayerDetailsCache = fileObject.Result.Select(playerDetails =>
                {
                    var details = new PlayerDetails
                    {
                        Handicap101 = playerDetails.Handicap101,
                        Handicap201 = playerDetails.Handicap201,
                        Handicap301 = playerDetails.Handicap301,
                        Handicap401 = playerDetails.Handicap401,
                        Handicap501 = playerDetails.Handicap501,
                        HandicapCricket = playerDetails.HandicapCricket,
                        Name = playerDetails.Name,
                        NickName = playerDetails.NickName,
                        Id = playerDetails.Id
                    };
                    var definition = new ImageDefinition
                    {
                        Name = playerDetails.PlayerImageDefinition.Name,
                        SourceBytes = playerDetails.PlayerImageDefinition.SourceBytes
                    };
                    details.PlayerImageDefinition = definition;
                    details.SelectedFlight = playerDetails.SelectedFlight;
                    return details;
                }).ToList();
            }


            // Regenerate seriaizable Images
            foreach (var playerDetails in PlayerDetailsCache)
            {
                playerDetails.RegenerateImage();
            }

            return PlayerDetailsCache.OrderBy(m => m.NickName).ToList();
        }

        private static void EnsureCache()
        {
            if (PlayerDetailsCache == null)
            {
                PlayerDetailsCache = new List<PlayerDetails>();
            }
        }
    }
}
