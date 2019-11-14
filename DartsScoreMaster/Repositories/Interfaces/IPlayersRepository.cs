using System;
using System.Collections.Generic;
using DartsScoreMaster.Model;

namespace DartsScoreMaster.Repositories.Interfaces
{
    public interface IPlayersRepository
    {
        List<PlayerDetails> GetAll();

        void Add(PlayerDetails playerDetails);

        void DeleteById(Guid playerDetailsId);

        void SaveAll();
    }
}