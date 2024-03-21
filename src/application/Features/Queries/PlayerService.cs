using application.Interfaces;
using application.Models.Response.Player;
using csgo_stats.Models;
using Microsoft.Extensions.DependencyInjection;

namespace application.Features.Queries
{
    public class PlayerService : IPlayerService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IPlayerMapper _playerMapper;

        public PlayerService(IServiceProvider serviceProvider, IPlayerMapper playerMapper)
        {
            _serviceProvider = serviceProvider;
            _playerMapper = playerMapper;
        }

        public IEnumerable<PlayerResponse> GetAllPlayers()
        {
            List<PlayerResponse> response = new List<PlayerResponse>();
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CsgoDbContext>();

                var players = dbContext.Player.Select(x => x);
                foreach (var player in players)
                {
                    var playerResponse = GetPlayerStadistics(player);

                    response.Add(playerResponse);
                }
            }

            return response;
        }

        public PlayerResponse? GetPlayerById(int? id)
        {
            PlayerResponse? response;
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CsgoDbContext>();

                var player = dbContext.Player
                    .Where(x => x.Id == id)
                    .FirstOrDefault();

                if (player is null)
                {
                    return null;
                }

                response = GetPlayerStadistics(player);
            }

            return response;
        }

        public PlayerResponse? GetPlayerByName(string? nickName)
        {
            PlayerResponse? response;
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CsgoDbContext>();

                var player = dbContext.Player
                    .Where(x => x.NickName.ToUpper() == nickName!.ToUpper())
                    .FirstOrDefault();

                if (player is null)
                {
                    return null;
                }

                response = GetPlayerStadistics(player);

            }

            return response;
        }

        public IEnumerable<RankingResponse> GetTopPlayersStats()
        {
            List<RankingResponse> response = new List<RankingResponse>();
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CsgoDbContext>();

                var killsPerPlayer = dbContext.Kills
                    .GroupBy(k => k.PlayerId)
                    .Select(x => new
                    {
                        Id = x.FirstOrDefault().PlayerId,
                        TotalKills = x.Sum(y => y.KillCount)
                    })
                    .OrderByDescending(x => x.TotalKills).ToList();

                if (killsPerPlayer is null)
                {
                    return Enumerable.Empty<RankingResponse>();
                }

                var topTen = killsPerPlayer.Take(10).ToList();

                for (int i = 1; i <= topTen.Count; i++)
                {
                    var player = topTen.ElementAtOrDefault(i - 1);
                    var playerResult = dbContext.Player.FirstOrDefault(x => x.Id == player.Id);
                    var team = dbContext.Team.FirstOrDefault(x => x.Id == playerResult.TeamId);

                    response.Add(_playerMapper.MapRankingResponse(playerResult, player.TotalKills, team, i));
                }

                return response;
            }
        }

        private PlayerResponse GetPlayerStadistics(Player player)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CsgoDbContext>();

                var matches = dbContext.Match
                    .Where(x => x.WinnerTeam == player.TeamId || x.WinnerTeam == player.TeamId);

                var MVPs = matches.Where(m => m.MVP == player.Id).Count();

                var matchHistory = matches
                    .Select(m => m.WinnerTeam == player.TeamId ? m.Id : -1);

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

                var highestScore = dbContext.Kills
                    .Where(k => k.PlayerId == player.Id)
                    .Max(k => k.KillCount);

                var team = dbContext.Team.FirstOrDefault(t => t.Id == player.TeamId)?.Name;

                return _playerMapper.MapPlayerResponse(player.NickName, matchHistory.Count(), totalWinnedGames, winStreak, MVPs, highestScore, player.Image, team!);
            }
        }
    }
}