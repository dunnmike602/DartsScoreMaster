using System;
using System.Collections.Generic;
using System.Linq;
using DartsScoreMaster.Model;
using DartsScoreMaster.Repositories.Interfaces;
using DartsScoreMaster.Services.Interfaces;

namespace DartsScoreMaster.Services
{
    public class CricketStatisticsCalculationService : IStatisticsCalculationService
    {
        private readonly IStatisticsRepository _statisticsRepository;

        public CricketStatisticsCalculationService(IStatisticsRepository statisticsRepository)
        {
            _statisticsRepository = statisticsRepository;
        }

        public void CalculatePlayerStatistics(GameType gameId, List<Player> players)
        {
            if (players.Count > 0)
            {
                // All summary records
                var playerSummaries = _statisticsRepository.GetForPlayers(players.Where(m => m.Scores.Any()).ToList()).
                    Select(m => m.Value);

                var statistics = CalculateStatisticsHelper(gameId, playerSummaries, players);

                _statisticsRepository.Add(statistics.ToList());

                _statisticsRepository.SaveAll();
            }
        }


        private StatisticsSummary ProcessStatistic(StatisticsSummary input, GameType gameId, Player player)
        {
            var currentStatisticSummary = input.Clone();

            if (player != null)
            {
                // Statistic for the day we are playing on    
                var currentStatistic = currentStatisticSummary.CreateLatestStatisticForGameType(gameId);

                var previousStatistic = currentStatisticSummary.GetPreviousStatisticForGame(gameId, SystemTime.Now())
                                        ?? currentStatistic;

                currentStatistic.Do(stat =>
                {
                    stat.MatchesWon = player.IsWinner
                        ? previousStatistic.MatchesWon + 1
                        : stat.MatchesWon;

                    stat.MatchesPlayed = previousStatistic.MatchesPlayed + 1;

                    stat.LegsWon = previousStatistic.LegsWon + player.CummulativeLegsWon;
                    stat.LegsPlayed = previousStatistic.LegsPlayed + player.LegsPlayed;

                    var lastDartAverage = previousStatistic.OneDartAverage;
                    var lastDartsThrown = previousStatistic.DartsThrown;

                    stat.OneDartAverage = stat.DartsThrown > 0
                        ? ((lastDartAverage * lastDartsThrown) +
                           player.Scores.Sum(m => m.Value))
                          / (stat.DartsThrown + player.Scores.Count)
                        : (decimal)player.Scores.Average(m => m.Value);

                    var maxTurnScoreInGames = player.TurnScores.Max(m => m.DartScore);
                    stat.HighestScore = maxTurnScoreInGames > stat.HighestScore
                        ? maxTurnScoreInGames
                        : 0;

                    stat.HundredsScored = previousStatistic.HundredsScored +
                                          player.TurnScores.Count(
                                              m => m.DartScore >= 100 && m.DartScore < 140);
                    stat.HundredFortiesScored = previousStatistic.HundredFortiesScored +
                                                player.TurnScores.Count(
                                                    m => m.DartScore >= 140 && m.DartScore < 180);
                    stat.HundredEightiesScored = previousStatistic.HundredEightiesScored +
                                                 player.TurnScores.Count(
                                                     m => m.DartScore == 180);

                    var winningLegs = player.Scores.Select(score => score.ScoredInLeg).Where(leg => leg.IsWinner);

                    stat.DartsThrownInWinningLegs = previousStatistic.DartsThrownInWinningLegs +
                                                    player.Scores.Count(score => winningLegs.Count(leg => leg.Id == score.ScoredInLeg.Id) > 0);
                });
            }

            return currentStatisticSummary;
        }

        private IEnumerable<StatisticsSummary> CalculateStatisticsHelper(GameType gameId, IEnumerable<StatisticsSummary> playerSummaries,
                IEnumerable<Player> players)
        {
            return playerSummaries.Select(stats => ProcessStatistic(stats, gameId, players.FirstOrDefault(m => m.PlayerDetails.Id == stats.PlayerId)));
        }

    }
}
