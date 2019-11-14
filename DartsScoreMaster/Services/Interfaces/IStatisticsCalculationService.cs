using System.Collections.Generic;
using DartsScoreMaster.Model;

namespace DartsScoreMaster.Services.Interfaces
{
    public interface IStatisticsCalculationService
    {
        void CalculatePlayerStatistics( GameType gameId, List<Player> players);
    }
}