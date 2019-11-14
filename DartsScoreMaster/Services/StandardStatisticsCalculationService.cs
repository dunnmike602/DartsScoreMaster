using System;
using System.Collections.Generic;
using System.Linq;
using DartsScoreMaster.Model;
using DartsScoreMaster.Repositories.Interfaces;
using DartsScoreMaster.Services.Interfaces;

namespace DartsScoreMaster.Services
{
    public class StandardStatisticsCalculationService : IStatisticsCalculationService
    {
        private readonly IStatisticsRepository _statisticsRepository;

        public StandardStatisticsCalculationService(IStatisticsRepository statisticsRepository)
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

        private StatisticsSummary ProcessStatistic(StatisticsSummary input, GameType gameId,
           Player player)
        {
            var currentStatisticSummary = input.Clone();

            if (player != null)
            {
                // Statistic for the day we are playing on    
                var previousStatistic = currentStatisticSummary.GetPreviousStatisticForGame(gameId, SystemTime.Now());

                var currentStatistic = currentStatisticSummary.CreateLatestStatisticForGameType(gameId);

                if (previousStatistic == null)
                {
                    previousStatistic = currentStatistic;
                }

                currentStatistic.Do(statistic =>
                {
                    statistic.MatchesWon = previousStatistic.MatchesWon;

                    if (player.IsWinner)
                    {
                        statistic.MatchesWon++;
                    }

                    var newBestGame = player.GetDartBestCheckOut();

                    statistic.BestGame = previousStatistic.BestGame;

                    if ((newBestGame != 0 && newBestGame < previousStatistic.BestGame) || previousStatistic.BestGame == 0)
                    {
                        statistic.BestGame = newBestGame;
                    }

                    statistic.TwelveDartCheckouts = previousStatistic.TwelveDartCheckouts +
                                               player.GetDartCheckOuts(10, 12);
                    statistic.NineDartCheckouts = previousStatistic.NineDartCheckouts +
                                             player.GetDartCheckOuts(1, 9);

                    statistic.MatchesPlayed = previousStatistic.MatchesPlayed + 1;

                    statistic.LegsWon = previousStatistic.LegsWon + player.CummulativeLegsWon;
                    statistic.LegsPlayed = previousStatistic.LegsPlayed + player.LegsPlayed;

                    var lastDartAverage = previousStatistic.OneDartAverage;
                    var lastDartsThrown = previousStatistic.DartsThrown;

                    if (statistic.DartsThrown > 0)
                    {
                        statistic.OneDartAverage = ((lastDartAverage * lastDartsThrown) +
                                               player.Scores.Sum(m => m.Value))
                                              / (statistic.DartsThrown + player.Scores.Count);
                    }
                    else
                    {
                        statistic.OneDartAverage = (decimal)player.Scores.Average(m => m.Value);
                    }

                    statistic.HighestScore = previousStatistic.HighestScore;

                    if (player.TurnScores.Max(m => m.DartScore) > statistic.HighestScore)
                    {
                        statistic.HighestScore = player.TurnScores.Max(m => m.DartScore);
                    }

                    var isWinner = player.TurnScores.FirstOrDefault(m => m.ScoredInLeg.IsWinner) != null;

                    statistic.HighestCheckout = previousStatistic.HighestCheckout;

                    if (isWinner &&
                        player.TurnScores.Where(m => m.ScoredInLeg.IsWinner).Max(m => m.DartScore) >
                        statistic.HighestCheckout)
                    {
                        statistic.HighestCheckout =
                            player.TurnScores.Where(m => m.ScoredInLeg.IsWinner).Max(m => m.DartScore);
                    }

                    statistic.HundredsScored = previousStatistic.HundredsScored +
                                          player.TurnScores.Count(
                                              m => m.DartScore >= 100 && m.DartScore < 140);
                    statistic.HundredFortiesScored = previousStatistic.HundredFortiesScored +
                                                player.TurnScores.Count(
                                                    m => m.DartScore >= 140 && m.DartScore < 180);
                    statistic.HundredEightiesScored = previousStatistic.HundredEightiesScored +
                                                 player.TurnScores.Count(m => m.DartScore == 180);

                    statistic.CheckoutPossibleCount = previousStatistic.CheckoutPossibleCount + player.TurnScores.Count(m => m.GetCanCheckout());

                    statistic.CheckoutAchievedCount = previousStatistic.CheckoutAchievedCount + player.GetDartCheckOuts(1, int.MaxValue);

                    statistic.DartsThrown = previousStatistic.DartsThrown + player.Scores.Count;

                    var winningLegs = player.Scores.Select(score => score.ScoredInLeg).Where(leg => leg.IsWinner);

                    statistic.DartsThrownInWinningLegs = previousStatistic.DartsThrownInWinningLegs +
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
