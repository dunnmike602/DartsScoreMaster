using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using DartsScoreMaster.Common;
using DartsScoreMaster.Model;
using DartsScoreMaster.Repositories.Interfaces;
using DartsScoreMaster.Repositories.Serialization.Interfaces;

namespace DartsScoreMaster.Repositories
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly ISerialization<StatisticsSummary> _statisticsSerialiser;
        private readonly IPlayersRepository _playersRepository;

        public StatisticsRepository(ISerialization<StatisticsSummary> statisticsSerialiser, IPlayersRepository playersRepository)
        {
            _statisticsSerialiser = statisticsSerialiser;
            _playersRepository = playersRepository;

            _statisticsSerialiser.Folder = ApplicationData.Current.LocalFolder;
        }

        private static Dictionary<Guid, StatisticsSummary> _statisticsCache;

        private void EnsureCache(IEnumerable<StatisticsSummary> statistics)
        {
            _statisticsCache = _statisticsCache ?? new Dictionary<Guid, StatisticsSummary>();

            foreach (var statistic in statistics)
            {
                if (!_statisticsCache.ContainsKey(statistic.PlayerId))
                {
                    if (statistic.PlayerId == Guid.Empty)
                    {
                        throw new Exception("INVALID PLAYER NAME");
                    }

                    _statisticsSerialiser.FileName = statistic.PlayerId + ".XML";
                    var loadedStatistic =  _statisticsSerialiser.Deserialize().Result;

                    _statisticsCache.Add(statistic.PlayerId, loadedStatistic ?? new StatisticsSummary {PlayerId = statistic.PlayerId});
                }
            }
        }


        public Dictionary<Guid, StatisticsSummary> GetForPlayers(List<Player> players)
        {
            var statistics = players.Select(player => new StatisticsSummary 
            {PlayerId = player.PlayerDetails.Id}).ToList();

            EnsureCache(statistics);

            return _statisticsCache;
        }


        public async Task<string> GetAllForDownload()
        {
            var result = new StringBuilder();

            result.Append("Name,NickName,Handicap501,Handicap401,Handicapt301,Handicap201,Handicap101,HandicapCricket");
            result.Append(",GameType,Date,CheckoutAchievedCount,CheckoutPossibleCount,DartsThrown,Hi3Darts,HiCheckOut");
            result.Append(",NineDartCheckouts,TwelveDartCheckouts,LowestDarts");
            result.Append(",HundredEightiesScored,HundredFortiesScored,HundredsScored,LegsPlayed,LegsWon,MatchesPlayed,MatchesWon,OneDartAverage,DartsThrownInWinningLegs");
            result.Append(Environment.NewLine);

            foreach (var player in _playersRepository.GetAll())
            {
                _statisticsSerialiser.FileName = player.Id + ".XML";

                var loadedStatistic = await _statisticsSerialiser.Deserialize() ?? new StatisticsSummary();

                foreach (var gameStatistic in loadedStatistic.GameStatistics)
                {

                    if (gameStatistic.Value.Count == 0)
                    {
                        gameStatistic.Value.Add(new Statistic());
                    }

                    foreach (var statistic in gameStatistic.Value)
                    {
                        result.Append(player.Name);
                        result.Append(",");
                        result.Append(player.NickName);
                        result.Append(",");
                        result.Append(player.Handicap501);
                        result.Append(",");
                        result.Append(player.Handicap401);
                        result.Append(",");
                        result.Append(player.Handicap301);
                        result.Append(",");
                        result.Append(player.Handicap201);
                        result.Append(",");
                        result.Append(player.Handicap101);
                        result.Append(",");
                        result.Append(player.HandicapCricket);
                        result.Append(",");
                        result.Append(gameStatistic.Key.GetAttribute<DisplayAttribute>().Name);
                        result.Append(",");
                        result.Append(statistic.Date.ToLocalTime());
                        result.Append(",");
                        result.Append(statistic.CheckoutAchievedCount);
                        result.Append(",");
                        result.Append(statistic.CheckoutPossibleCount);
                        result.Append(",");
                        result.Append(statistic.DartsThrown);
                        result.Append(",");
                        result.Append(statistic.HighestScore);
                        result.Append(",");
                        result.Append(statistic.HighestCheckout);
                        result.Append(",");
                        result.Append(statistic.NineDartCheckouts);
                        result.Append(",");
                        result.Append(statistic.TwelveDartCheckouts);
                        result.Append(",");
                        result.Append(statistic.BestGame);
                        result.Append(",");
                        result.Append(statistic.HundredEightiesScored);
                        result.Append(",");
                        result.Append(statistic.HundredFortiesScored);
                        result.Append(",");
                        result.Append(statistic.HundredsScored);
                        result.Append(",");
                        result.Append(statistic.LegsPlayed);
                        result.Append(",");
                        result.Append(statistic.LegsWon);
                        result.Append(",");
                        result.Append(statistic.MatchesPlayed);
                        result.Append(",");
                        result.Append(statistic.MatchesWon);
                        result.Append(",");
                        result.Append(statistic.OneDartAverage);
                        result.Append(",");
                        result.Append(statistic.DartsThrownInWinningLegs);
                        result.Append(Environment.NewLine);
                    }
                }
            }

            return result.ToString();
        }
        
        public async Task DeleteByPlayerId(Guid playerDetailsId)
        {
            var currentItem = _statisticsCache?[playerDetailsId];

            if (currentItem != null)
            {
                _statisticsCache.Remove(playerDetailsId);
                var file = await ApplicationData.Current.LocalFolder.TryGetItemAsync(playerDetailsId + ".XML");

                if (file != null)
                {
                    await file.DeleteAsync();
                }
            }
        }

        public StatisticsSummary GetForPlayer(Guid playerId)
        {
            EnsureCache(new[] {new StatisticsSummary { PlayerId = playerId }});

            return _statisticsCache.FirstOrDefault(m => m.Key == playerId).Value;
        }

        public void Add(List<StatisticsSummary> statistics)
        {
            EnsureCache(statistics);

            foreach (var statistic in statistics)
            {
                if (_statisticsCache.ContainsKey(statistic.PlayerId))
                {
                    _statisticsCache[statistic.PlayerId] = statistic;
                }
                else
                {
                    _statisticsCache.Add(statistic.PlayerId, statistic);
                }
            }
        }

        public async void SaveAll()
        {
            foreach (var statistic in _statisticsCache)
            {
                if (statistic.Value.PlayerId == Guid.Empty)
                {
                    throw new Exception("INVALID PLAYER NAME");
                }

                _statisticsSerialiser.FileName = statistic.Value.PlayerId + ".XML";

               await _statisticsSerialiser.Serialize(statistic.Value);
            }
          
        }
    }
}