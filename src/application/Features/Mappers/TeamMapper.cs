using application.Interfaces;
using application.Models.Response.Team;
using csgo_stats.Models;

namespace application.Features.Mappers
{
    public class TeamMapper : ITeamMapper
    {
        public TeamResponse MapTeamResponse(Team team, string mvp, int totalWinnedGames, int winStreak, IEnumerable<Player> players) =>
            new TeamResponse()
            {
                TeamId = team.Id,
                Name = team.Name,
                Logo = team.Logo,
                HighestWinStreak = winStreak,
                WinnedGames = totalWinnedGames,
                TeamMVP = mvp,
                Players = players.Select(p => new TeamPlayer() { Id = p.Id, Name = p.NickName })
            };

        public TeamRankingResponse MapRankingResponse(IEnumerable<string> players, int totalWins, Team team, int i) =>
            new TeamRankingResponse()
            {
                Ranking = i,
                TeamName = team.Name,
                TeamLogo = team.Logo,
                TeamMembers = players,
                WinnedGames = totalWins
            };
    }
}