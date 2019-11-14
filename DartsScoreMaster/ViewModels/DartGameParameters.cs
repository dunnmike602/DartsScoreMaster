using System;
using DartsScoreMaster.Model;
using ReactiveUI;

namespace DartsScoreMaster.ViewModels
{
    public class DartGameParameters
    {
        public ReactiveList<Player> Players { get; set; }
        public int CurrentPlayer { get; set; }
        public int DartTotal { get; set; }
        public Score DartScore { get; set; }
        public bool IsBust { get; set; }
        public Guid CurrentGameId { get; set; }
        public int CurrentTurn { get; set; }
        public bool IsGameOver { get; set; }
    }
}