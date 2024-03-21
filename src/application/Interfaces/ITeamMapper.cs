using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using application.Models.Response.Team;
using csgo_stats.Models;

namespace application.Interfaces
{
    public interface ITeamMapper
    {
        TeamRankingResponse MapRankingResponse(IEnumerable<string> players, int totalWins, Team team, int i);
        TeamResponse MapTeamResponse(Team team, string mvp, int totalWinnedGames, int winStreak, IEnumerable<Player> players);
    }
}