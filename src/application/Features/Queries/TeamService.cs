using application.Interfaces;
using application.Models.Response.Team;
using csgo_stats.Models;
using Microsoft.Extensions.DependencyInjection;

namespace application.Features.Queries
{
    public class TeamService : ITeamService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ITeamMapper _teamMapper;

        public TeamService(IServiceProvider serviceProvider, ITeamMapper teamMapper)
        {
            _serviceProvider = serviceProvider;
            _teamMapper = teamMapper;
        }
        public TeamResponse? GetTeamById(int? id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CsgoDbContext>();

                var team = dbContext.Team.FirstOrDefault(t => t.Id == id);

                if (team is null)
                {
                    return null;
                }
                return GetTeamStadistics(team);
            }
        }

        public TeamResponse? GetTeamByName(string? name)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CsgoDbContext>();

                var team = dbContext.Team.FirstOrDefault(t => t.Name.ToUpper() == name!.ToUpper());

                if (team is null)
                {
                    return null;
                }

                return GetTeamStadistics(team);
            }
        }

        public IEnumerable<TeamRankingResponse> GetRankingTeamResponse()
        {
            List<TeamRankingResponse> response = new List<TeamRankingResponse>();
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CsgoDbContext>();

                var winsPerTeam = dbContext.Match
                    .GroupBy(m => m.WinnerTeam)
                    .Select(x => new
                    {
                        TeamId = x.FirstOrDefault().Id,
                        TotalWins = x.Count()
                    })
                    .OrderByDescending(x => x.TotalWins).ToList();

                if (winsPerTeam is null)
                {
                    return Enumerable.Empty<TeamRankingResponse>();
                }

                var topTeams = winsPerTeam.Take(10).ToList();

                for (int i = 1; i <= topTeams.Count; i++)
                {
                    var team = topTeams.ElementAtOrDefault(i - 1);
                    var teamResult = dbContext.Team.FirstOrDefault(x => x.Id == team.TeamId);
                    var players = dbContext.Player.Where(x => x.TeamId == teamResult.Id).Select(p => p.NickName).ToList();

                    response.Add(_teamMapper.MapRankingResponse(players, team.TotalWins, teamResult, i));
                }

                return response;
            }
        }

        private TeamResponse GetTeamStadistics(Team team)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CsgoDbContext>();

                var players = dbContext.Player.Where(p => p.TeamId == team.Id).ToList();
                var matches = dbContext.Match.Where(m => m.WinnerTeam == team.Id || m.LoserTeam == team.Id).ToList();

                var matchHistory = matches.Select(m => m.WinnerTeam == team.Id ? team.Id : -1).ToList();

                var totalWinnedGames = matchHistory.Count(m => m != -1);

                var winStreak = 0;
                var currentStreak = 0;
                foreach (var match in matchHistory)
                {
                    if (match > -1)
                    {
                        currentStreak++;
                    }
                    else
                    {
                        currentStreak = 0;
                    }

                    if (currentStreak > winStreak)
                    {
                        winStreak = currentStreak;
                    }
                }

                var MVPValue = 0;
                var MVPPlayer = 0;
                foreach (var player in players)
                {
                    var mvpCounter = 0;
                    foreach (var match in matches)
                    {
                        if (match.MVP == player.Id) mvpCounter++;
                    }

                    if (mvpCounter > MVPValue)
                    {
                        MVPPlayer = player.Id;
                        MVPValue = mvpCounter;
                    }
                }

                if (MVPPlayer == 0) MVPPlayer = players.FirstOrDefault().Id;

                var mvpPlayerName = players.FirstOrDefault(p => p.Id == MVPPlayer).NickName;

                return _teamMapper.MapTeamResponse(team, mvpPlayerName, totalWinnedGames, winStreak, players);
            }
        }
    }
}