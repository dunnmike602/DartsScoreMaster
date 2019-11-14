using System;
using System.Collections.Generic;
using System.Linq;
using DartsScoreMaster.Model;
using DartsScoreMaster.ViewModels.Interfaces;
using ReactiveUI;

namespace DartsScoreMaster.ViewModels
{
    public class Cricket : IDartGame
    {
        public const int HitsToOpenCloseScore = 3;

        private readonly List<int> _numberSlots = new List<int>{20,19,18,17,16,15,25};
        private readonly List<int> _validScores = new List<int> { 20, 19, 18, 17, 16, 15, 25 , 50};

        public bool HasAnyOneScored(List<Player> players)
        {
            return players.Any(player => player.Scores.Count + player.LastDarts.Count > 0);
        }

        public List<Tuple<int, string>> GetCheckOutHints(DartGameParameters parameters)
        {
            return new List<Tuple<int, string>>();
        }

        private List<Player> GetWinningPlayers(List<Player> players )
        {
            var maxScore = players.Max(m => m.GetCurrentCummulativeScore());

            return players.Where(m => m.GetCurrentCummulativeScore() == maxScore).ToList();
        }

        public bool IsGameOver(DartGameParameters parameters)
        {
            var isAnyOpenScore = false;

            var winners = GetWinningPlayers(parameters.Players.ToList());

            if (winners.Count == 1)
            {
                var currentPlayer = parameters.Players[parameters.CurrentPlayer];

                var leader = winners.First();

                var currentPlayerIsLeader = currentPlayer.UniqueId == leader.UniqueId;

                if (currentPlayerIsLeader && ScorableCount(new List<Player>{currentPlayer}) > 0 || ScorableCount(parameters.Players) > 2)
                {
                    isAnyOpenScore = true;
                }

                if (!currentPlayerIsLeader && ScorableCount(new List<Player> { currentPlayer }) == 0 || ScorableCount(parameters.Players) > 1)
                {
                    isAnyOpenScore = true;
                }
            }
            else
            {
                isAnyOpenScore = ScorableCount(parameters.Players) > 0;
            }

            return !isAnyOpenScore;
        }

        public bool IsValidScore(DartGameParameters parameters)
        {
            return true;
        }

        public string ProcessScore(DartGameParameters parameters)
        {
            var currentPlayer = parameters.Players[parameters.CurrentPlayer];
            currentPlayer.InterimScore = currentPlayer.CurrentScore + parameters.DartTotal;

            return parameters.DartTotal.ToString();
        }

        public int ProcessTotal(DartGameParameters parameters)
        {
            var currentScore = parameters.DartScore;

            // Is this a scoring opportunity
            if (_validScores.Any(m => m == currentScore.NumberHit))
            {
                // Convert a bull to a double 25 - makes handling this score easier
                if (currentScore.ScoreType == ScoreType.Bull)
                {
                    currentScore.Value = 50;
                    currentScore.NumberHit = 25;
                    currentScore.ScoreType = ScoreType.Double;
                }

                Player currentPlayer = parameters.Players[parameters.CurrentPlayer];

                var startingHitCount = currentPlayer.GetNumberHitCount(currentScore.NumberHit);

                if (!IsScoreable(currentScore.NumberHit, parameters.Players.Except(new List<Player>{currentPlayer})))
                {
                    currentPlayer.SetNumberHitCount(currentScore.NumberHit, Math.Min(GetScoreMultiplier(currentScore) + startingHitCount, 3));
                    currentScore.Value = 0;
                    return 0;
                }

                if (startingHitCount == HitsToOpenCloseScore)
                {
                    return parameters.DartScore.Value;
                }

                var scoreInstances = GetScoreMultiplier(currentScore);

                currentPlayer.SetNumberHitCount(currentScore.NumberHit, Math.Min(scoreInstances + startingHitCount, 3));

                var endingHitCount = currentPlayer.GetNumberHitCount(currentScore.NumberHit);

                var scoreMultiplier = endingHitCount - startingHitCount;

                currentScore.Value = scoreMultiplier > 0 ? parameters.DartScore.Value - (currentScore.NumberHit * scoreMultiplier) : 0;

                return  currentScore.Value;
            }

            currentScore.Value = 0;

            return 0;
        }

        public int GetScoreMultiplier(Score currentScore)
        {
            switch (currentScore.ScoreType)
            {
                case ScoreType.Single:
                    return 1;
                case ScoreType.Double:
                    return 2;
                case ScoreType.Treble:
                    return 3;
                case ScoreType.OuterBull:
                    return 1;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CheckGamesState(ReactiveList<Player> players)
        {
            foreach (var validScore in _numberSlots)
            {
                var allClosed = players.All(m => m.GetNumberHitCount(validScore) == 3);

                foreach (var player in players)
                {
                    player.SetNumberColor(validScore, allClosed);
                }
            }
        }

        public void AcceptScore(DartGameParameters parameters)
        {
            var currentPlayer = parameters.Players[parameters.CurrentPlayer];

            var undoItem = new UndoItem
            {
                Score = currentPlayer.CurrentScore,
                CurrentDartCount = currentPlayer.CurrentDartCount - currentPlayer.LastDarts.Count,
            };

            currentPlayer.UndoItems.Add(undoItem);

            undoItem.HitsBull = currentPlayer.OldHits.ContainsKey(25)
                ? currentPlayer.OldHits[25]
                : currentPlayer.HitsBull;

            undoItem.Hits20 = currentPlayer.OldHits.ContainsKey(20) ? currentPlayer.OldHits[20] : currentPlayer.Hits20;
            undoItem.Hits19 = currentPlayer.OldHits.ContainsKey(19) ? currentPlayer.OldHits[19] : currentPlayer.Hits19;
            undoItem.Hits18 = currentPlayer.OldHits.ContainsKey(18) ? currentPlayer.OldHits[18] : currentPlayer.Hits18;
            undoItem.Hits17 = currentPlayer.OldHits.ContainsKey(17) ? currentPlayer.OldHits[17] : currentPlayer.Hits17;
            undoItem.Hits16 = currentPlayer.OldHits.ContainsKey(16) ? currentPlayer.OldHits[16] : currentPlayer.Hits16;
            undoItem.Hits15 = currentPlayer.OldHits.ContainsKey(15) ? currentPlayer.OldHits[15] : currentPlayer.Hits15;

            currentPlayer.AcceptCricketScores();

            currentPlayer.CurrentScore += parameters.DartTotal;

            currentPlayer.MergeLastDarts(false, new Leg
            {
                Id = parameters.CurrentGameId,
                IsWinner = parameters.IsGameOver

            }, parameters.CurrentTurn);

            CheckGamesState(parameters.Players);
        }

        public void RejectScore(DartGameParameters parameters)
        {
            var currentPlayer = parameters.Players[parameters.CurrentPlayer];

            currentPlayer.DiscardLastDarts();
            currentPlayer.ResetCricketScores();

            currentPlayer.InterimScore = currentPlayer.CurrentScore;
        }

        public List<Player> GetVictoriousPlayers(DartGameParameters gameParameters)
        {
            var winningPlayers =  GetWinningPlayers(gameParameters.Players.ToList());

            foreach (var winningPlayer in winningPlayers)
            {
                winningPlayer.IsWinner = true;
            }

            return winningPlayers;
        }

        public void Undo(DartGameParameters gameParameters)
        {
            var playerToUndo = gameParameters.Players[gameParameters.CurrentPlayer];

            var undoItem = playerToUndo.PopUndo();

            if (undoItem != null)
            {
                playerToUndo.CurrentScore = undoItem.Score;
                playerToUndo.InterimScore = undoItem.Score;
                playerToUndo.CurrentDartCount = undoItem.CurrentDartCount;
                playerToUndo.Hits20 = undoItem.Hits20;
                playerToUndo.Hits19 = undoItem.Hits19;
                playerToUndo.Hits18 = undoItem.Hits18;
                playerToUndo.Hits17 = undoItem.Hits17;
                playerToUndo.Hits16 = undoItem.Hits16;
                playerToUndo.Hits15 = undoItem.Hits15;
                playerToUndo.HitsBull = undoItem.HitsBull;

                CheckGamesState(gameParameters.Players);
            }
        }

        public void CloseMatch(DartGameParameters gameParameters)
        {
            foreach (var player in gameParameters.Players)
            {
                player.ClearUndo();
            }
        }

        private int ScorableCount(IList<Player> players)
        {
            var scoreCount = 0;

            foreach (var player in players)
            {
                foreach (var numberHit in _numberSlots)
                {
                    if (player.GetNumberHitCount(numberHit) < HitsToOpenCloseScore)
                    {
                        scoreCount++;
                        break;
                    }
                }
            }
            return scoreCount;
        }

        private static bool IsScoreable(int numberHit, IEnumerable<Player> players)
        {
            // Number must not be closed by all players for it to be scored on
            return players.Any(m => m.GetNumberHitCount(numberHit) < HitsToOpenCloseScore);
        }
    }
}