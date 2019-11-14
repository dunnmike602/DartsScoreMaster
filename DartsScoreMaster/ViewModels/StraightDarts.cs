using System;
using System.Collections.Generic;
using System.Linq;
using DartsScoreMaster.Model;
using DartsScoreMaster.ViewModels.Interfaces;

namespace DartsScoreMaster.ViewModels
{
    public class StraightDarts : IDartGame
    {
        private List<Tuple< int, string>> _checkoutCard;

        public StraightDarts()
        {
            SetupCheckOutCard();
        }

        
        private void SetupCheckOutCard()
        {
            _checkoutCard = new List<Tuple< int, string>>
            {
                new Tuple<int, string>(170, "T20 T20 Bull"),
                new Tuple<int, string>(167, "T20 T19 Bull"),
                new Tuple<int, string>(164, "T20 T18 Bull"),
                new Tuple<int, string>(161, "T20 T17 Bull"),
                new Tuple<int, string>(160, "T20 T20 D20"),
                new Tuple<int, string>(158, "T20 T20 D19"),
                new Tuple<int, string>(157, "T20 T19 D20"),
                new Tuple<int, string>(156, "T20 T20 D18"),
                new Tuple<int, string>(155, "T20 T19 D19"),
                new Tuple<int, string>(154, "T20 T18 D20"),
                new Tuple<int, string>(153, "T20 T19 D18"),
                new Tuple<int, string>(152, "T20 T20 D16"),
                new Tuple<int, string>(151, "T20 T17 D20"),
                new Tuple<int, string>(150, "T20 T18 D18"),
                new Tuple<int, string>(149, "T20 T19 D16"),
                new Tuple<int, string>(148, "T20 T16 D20"),
                new Tuple<int, string>(147, "T20 T17 D18"),
                new Tuple<int, string>(146, "T20 T18 D16"),
                new Tuple<int, string>(145, "T20 T15 D20"),
                new Tuple<int, string>(144, "T20 T20 D12"),
                new Tuple<int, string>(143, "T20 T17 D16"),
                new Tuple<int, string>(142, "T20 T14 D20"),
                new Tuple<int, string>(141, "T20 T15 D18"),
                new Tuple<int, string>(140, "T20 T16 D16"),
                new Tuple<int, string>(139, "T19 T14 D20"),
                new Tuple<int, string>(138, "T20 T18 D12"),
                new Tuple<int, string>(137, "T19 T16 D16"),
                new Tuple<int, string>(136, "T20 T20 D8"),
                new Tuple<int, string>(135, "T20 T15 D15"),
                new Tuple<int, string>(134, "T20 T14 D16"),
                new Tuple<int, string>(133, "T20 T19 D8"),
                new Tuple<int, string>(132, "T20 T16 D12"),
                new Tuple<int, string>(131, "T20 T13 D16"),
                new Tuple<int, string>(130, "T20 20 Bull"),
                new Tuple<int, string>(129, "T19 T16 D12"),
                new Tuple<int, string>(128, "T20 T20 D4"),
                new Tuple<int, string>(127, "T20 T17 D8"),
                new Tuple<int, string>(126, "T19 19 Bull"),
                new Tuple<int, string>(125, "T20 25 D20"),
                new Tuple<int, string>(124, "T20 T16 D8"),
                new Tuple<int, string>(123, "T19 T10 D18"),
                new Tuple<int, string>(122, "T20 T18 D4"),
                new Tuple<int, string>(121, "T17 T18 D8"),
                new Tuple<int, string>(120, "T20 20 D20"),
                new Tuple<int, string>(119, "T19 T10 D16"),
                new Tuple<int, string>(118, "T18 T16 D8"),
                new Tuple<int, string>(117, "T19 20 D20"),
                new Tuple<int, string>(116, "T20 20 D18"),
                new Tuple<int, string>(115, "T19 18 D20"),
                new Tuple<int, string>(114, "T20 18 D18"),
                new Tuple<int, string>(113, "T19 16 D20"),
                new Tuple<int, string>(112, "T20 20 D16"),
                new Tuple<int, string>(111, "T20 11 D20"),
                new Tuple<int, string>(111, "T19 14 D20"),
                new Tuple<int, string>(110, "T20 Bull"),
                new Tuple<int, string>(110, "T20 18 D16"),
                new Tuple<int, string>(109, "T20 9 D20"),
                new Tuple<int, string>(109, "T19 20 D16"),
                new Tuple<int, string>(108, "T20 16 D16"),
                new Tuple<int, string>(107, "T19 Bull"),
                new Tuple<int, string>(107, "T19 18 D16"),
                new Tuple<int, string>(106, "T20 T10 D8"),
                new Tuple<int, string>(106, "T20 14 D16"),
                new Tuple<int, string>(105, "T20 13 D16"),
                new Tuple<int, string>(105, "T19 16 D16"),
                new Tuple<int, string>(104, "T18 Bull"),
                new Tuple<int, string>(104, "T19 15 D16"),
                new Tuple<int, string>(103, "T17 20 D16"),
                new Tuple<int, string>(103, "T19 10 D18"),
                new Tuple<int, string>(102, "T20 10 D16"),
                new Tuple<int, string>(101, "T17 Bull"),
                new Tuple<int, string>(101, "T20 1 D20"),
                new Tuple<int, string>(100, "T20 D20"),
                new Tuple<int, string>(99, "T19 10 D16"),
                new Tuple<int, string>(98, "T20 D19"),
                new Tuple<int, string>(97, "T19 D20"),
                new Tuple<int, string>(96, "T20 D18"),
                new Tuple<int, string>(95, "T19 D19"),
                new Tuple<int, string>(95, "T20 25 D5"),
                new Tuple<int, string>(95, "25 20 Bull"),
                new Tuple<int, string>(94, "T18 D20"),
                new Tuple<int, string>(93, "T19 D18"),
                new Tuple<int, string>(92, "T20 D16"),
                new Tuple<int, string>(91, "T17 D20"),
                new Tuple<int, string>(90, "T18 D18"),
                new Tuple<int, string>(89, "T19 D16"),
                new Tuple<int, string>(88, "T16 D20"),
                new Tuple<int, string>(87, "T17 D18"),
                new Tuple<int, string>(86, "T18 D16"),
                new Tuple<int, string>(85, "T15 D20"),
                new Tuple<int, string>(84, "T20 D12"),
                new Tuple<int, string>(83, "T17 D16"),
                new Tuple<int, string>(82, "T14 D20"),
                new Tuple<int, string>(81, "T15 D18"),
                new Tuple<int, string>(80, "T16 D16"),
                new Tuple<int, string>(79, "T17 D14"),
                new Tuple<int, string>(78, "T18 D12"),
                new Tuple<int, string>(77, "T19 D10"),
                new Tuple<int, string>(76, "T20 D8"),
                new Tuple<int, string>(75, "T15 D15"),
                new Tuple<int, string>(74, "T14 D16"),
                new Tuple<int, string>(73, "T19 D8"),
                new Tuple<int, string>(72, "T16 D12"),
                new Tuple<int, string>(71, "T13 D16"),
                new Tuple<int, string>(70, "T10 D20"),
                new Tuple<int, string>(69, "T19 D6"),
                new Tuple<int, string>(68, "T20 D4"),
                new Tuple<int, string>(67, "T17 D8"),
                new Tuple<int, string>(66, "T10 D18"),
                new Tuple<int, string>(65, "25 D20"),
                new Tuple<int, string>(64, "T16 D8"),
                new Tuple<int, string>(63, "T17 D6"),
                new Tuple<int, string>(62, "T10 D16"),
                new Tuple<int, string>(61, "25 D18"),
                new Tuple<int, string>(60, "20 D20"),
                new Tuple<int, string>(59, "19 D20"),
                new Tuple<int, string>(58, "18 D20"),
                new Tuple<int, string>(57, "17 D20"),
                new Tuple<int, string>(56, "16 D20"),
                new Tuple<int, string>(55, "15 D20"),
                new Tuple<int, string>(54, "18 D18"),
                new Tuple<int, string>(53, "13 D20"),
                new Tuple<int, string>(52, "20 D16"),
                new Tuple<int, string>(51, "19 D16"),
                new Tuple<int, string>(50, "18 D16"),
                new Tuple<int, string>(49, "17 D16"),
                new Tuple<int, string>(48, "16 D16"),
                new Tuple<int, string>(47, "15 D16"),
                new Tuple<int, string>(46, "14 D16"),
                new Tuple<int, string>(45, "13 D16"),
                new Tuple<int, string>(44, "12 D16"),
                new Tuple<int, string>(43, "11 D16"),
                new Tuple<int, string>(42, "10 D16"),
                new Tuple<int, string>(41, "9 D16"),
                new Tuple<int, string>(40, "D20"),
                new Tuple<int, string>(40, "8 D16"),
                new Tuple<int, string>(39, "7 D16"),
                new Tuple<int, string>(39, "19 D10"),
                new Tuple<int, string>(39, "3 D18"),
                new Tuple<int, string>(38, "D19"),
                new Tuple<int, string>(37, "5 D16"),
                new Tuple<int, string>(37, "1 D18"),
                new Tuple<int, string>(37, "3 D14"),
                new Tuple<int, string>(36, "D18"),
                new Tuple<int, string>(35, "3 D16"),
                new Tuple<int, string>(34, "D17"),
                new Tuple<int, string>(34, "2 D16"),
                new Tuple<int, string>(33, "1 D16"),
                new Tuple<int, string>(33, "3 D15"),
                new Tuple<int, string>(32, "D16"),
                new Tuple<int, string>(31, "15 D8"),
                new Tuple<int, string>(30, "D15"),
                new Tuple<int, string>(29, "13 D8"),
                new Tuple<int, string>(28, "D14"),
                new Tuple<int, string>(27, "19 D4"),
                new Tuple<int, string>(27, "7 D10"),
                new Tuple<int, string>(26, "D13"),
                new Tuple<int, string>(25, "17 D4"),
                new Tuple<int, string>(25, "9 D8"),
                new Tuple<int, string>(24, "D12"),
                new Tuple<int, string>(23, "7 D8"),
                new Tuple<int, string>(22, "D11"),
                new Tuple<int, string>(21, "5 D8"),
                new Tuple<int, string>(21, "17 D2"),
                new Tuple<int, string>(20, "D10"),
                new Tuple<int, string>(19, "11 D4"),
                new Tuple<int, string>(19, "3 D8"),
                new Tuple<int, string>(18, "D9"),
                new Tuple<int, string>(17, "9 D4"),
                new Tuple<int, string>(17, "5 D6"),
                new Tuple<int, string>(16, "D8"),
                new Tuple<int, string>(15, "1 D7"),
                new Tuple<int, string>(15, "3 D6"),
                new Tuple<int, string>(14, "D7"),
                new Tuple<int, string>(13, "1 D6"),
                new Tuple<int, string>(13, "3 D5"),
                new Tuple<int, string>(12, "D6"),
                new Tuple<int, string>(11, "1 D5"),
                new Tuple<int, string>(11, "3 D4"),
                new Tuple<int, string>(10, "D5"),
                new Tuple<int, string>(9, "1 D4"),
                new Tuple<int, string>(8, "D4"),
                new Tuple<int, string>(7, "1 D3"),
                new Tuple<int, string>(6, "D3"),
                new Tuple<int, string>(5, "1 D2"),
                new Tuple<int, string>(4, "D2"),
                new Tuple<int, string>(3, "1 D1"),
                new Tuple<int, string>(2, "D1"),
            };
        }

        public bool HasAnyOneScored(List<Player> players)
        {
            return players.Any(player => player.Scores.Count + player.LastDarts.Count > 0);
        }

        public List<Tuple<int, string>> GetCheckOutHints(DartGameParameters parameters)
        {
            if (!parameters.Players.Any())
            {
                return new List<Tuple<int, string>>();
            }

            return
                _checkoutCard.Where(
                    m => m.Item1 == parameters.Players[parameters.CurrentPlayer].CurrentScore - parameters.DartTotal).ToList();
        }
        
        public List<Tuple<int, string>> GetCheckOutForNextTurn(DartGameParameters parameters)
        {
            if (!parameters.Players.Any())
            {
                return new List<Tuple<int, string>>();
            }

            return
                _checkoutCard.Where(
                    m =>
                        m.Item2.Split(' ').Length <= 3 &&
                        m.Item1 == parameters.Players[parameters.CurrentPlayer].CurrentScore - parameters.DartTotal).ToList();

        }

        public bool IsGameOver(DartGameParameters parameters)
        {
            return parameters.Players[parameters.CurrentPlayer].CurrentScore - parameters.DartTotal == 0 &&
                   (parameters.DartScore.ScoreType == ScoreType.Bull || parameters.DartScore.ScoreType == ScoreType.Double);
        }

        public bool IsValidScore(DartGameParameters parameters)
        {
            return parameters.Players[parameters.CurrentPlayer].CurrentScore - parameters.DartTotal >= 2;
        }

        public string ProcessScore(DartGameParameters parameters)
        {
            var currentPlayer = parameters.Players[parameters.CurrentPlayer];

            currentPlayer.InterimScore = currentPlayer.CurrentScore - parameters.DartTotal;

            if (currentPlayer.InterimScore > 1)
            {
                return parameters.DartTotal.ToString();
            }

            return string.Empty;
        }

        public int ProcessTotal(DartGameParameters parameters)
        {
            return parameters.DartScore.Value;
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

            if (parameters.IsBust)
            {
                // If player busts then no value is recorded
                currentPlayer.RemoveDartScores();
                currentPlayer.InterimScore = parameters.Players[parameters.CurrentPlayer].CurrentScore;
            }

            currentPlayer.MergeLastDarts(false, new Leg
            {
                Id = parameters.CurrentGameId,
                IsWinner = parameters.IsGameOver

            }, parameters.CurrentTurn);

            if (!parameters.IsBust)
            {
                parameters.Players[parameters.CurrentPlayer].CurrentScore -= parameters.DartTotal;
            }
        }

        public void RejectScore(DartGameParameters parameters)
        {
            var currentPlayer = parameters.Players[parameters.CurrentPlayer];

            currentPlayer.DiscardLastDarts();

            currentPlayer.InterimScore = currentPlayer.CurrentScore;
        }

        public List<Player> GetVictoriousPlayers(DartGameParameters getGameParameters)
        {
            return new List<Player> {getGameParameters.Players.First(m => m.CurrentScore == 0)};
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
            }
        }

        public void CloseMatch(DartGameParameters gameParameters)
        {
            foreach (var player in gameParameters.Players)
            {
                player.ClearUndo();
            }
        }
    }
}