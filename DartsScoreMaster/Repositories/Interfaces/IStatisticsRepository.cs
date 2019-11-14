using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DartsScoreMaster.Model;

namespace DartsScoreMaster.Repositories.Interfaces
{
    public interface IStatisticsRepository
    {
        void Add(List<StatisticsSummary> statistics);

        void SaveAll();

        Dictionary<Guid, StatisticsSummary> GetForPlayers(List<Player> players);

        StatisticsSummary GetForPlayer(Guid playerId);

        Task<string> GetAllForDownload();

        Task DeleteByPlayerId(Guid playerDetailsId);
    }
}