using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using DartsScoreMaster.FunctionalCore;

namespace DartsScoreMaster.Model
{
    [DataContract]
    public class StatisticsSummary : DeepClonable<StatisticsSummary>
    {
        public StatisticsSummary()
        {
            GameStatistics = new Dictionary<GameType, List<Statistic>>();

            for (var i = 0; i <= 5; i++)
            {
                GameStatistics[(GameType) i] = new List<Statistic>();
            }
        }


        public StatisticsSummary(Guid playerId, Dictionary<GameType, List<Statistic>> gameStatistics)
        {
            PlayerId = playerId;
            GameStatistics = gameStatistics;
        }

        [DataMember]
        public Guid PlayerId { get; set; }

        [DataMember]
        public Dictionary<GameType, List<Statistic>> GameStatistics { get; private set; }

        public override StatisticsSummary Clone()
        {
            var gameStatistics = GameStatistics.ToDictionary(x => x.Key,
                y => y.Value.Select(m => m.Clone()).ToList());

 
            return new StatisticsSummary(PlayerId, gameStatistics);
        }

        public Statistic GetLatestStatisticForGame(GameType gameType)
        {
            return GameStatistics[gameType].OrderBy(m => m.Date).LastOrDefault();
        }

        public Statistic GetStatisticForGameAndDate(GameType gameType, DateTime today)
        {
            return GameStatistics[gameType].LastOrDefault(m => m.Date == today);
        }


        public Statistic CreateLatestStatisticForGameType(GameType gameType)
        {
            var statistic = GetStatisticForGameAndDate(gameType, SystemTime.Now()) ?? new Statistic { Date = SystemTime.Now() };

           return GameStatistics[gameType].AddIfNotPresent(statistic,() => GameStatistics[gameType].All(m => m.Date != statistic.Date));
        }

        public Statistic GetPreviousStatisticForGame(GameType gameType, DateTime currentDate)
        {
            return GameStatistics[gameType].Where(m => m.Date < currentDate).OrderBy(m => m.Date).LastOrDefault();
        }
    }
}