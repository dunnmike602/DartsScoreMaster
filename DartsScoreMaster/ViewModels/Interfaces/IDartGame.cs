using System;
using System.Collections.Generic;
using DartsScoreMaster.Model;

namespace DartsScoreMaster.ViewModels.Interfaces
{
    public interface IDartGame
    {
        bool HasAnyOneScored(List<Player> players);

        List<Tuple<int, string>> GetCheckOutHints(DartGameParameters parameters);

        bool IsGameOver(DartGameParameters parameters);

        bool IsValidScore(DartGameParameters parameters);

        string ProcessScore(DartGameParameters parameters);

        int ProcessTotal(DartGameParameters parameters);

        void AcceptScore(DartGameParameters parameters);

        void RejectScore(DartGameParameters parameters);

        List<Player> GetVictoriousPlayers(DartGameParameters getGameParameters);

        void Undo(DartGameParameters gameParameters);

        void CloseMatch(DartGameParameters gameParameters);
    }
}