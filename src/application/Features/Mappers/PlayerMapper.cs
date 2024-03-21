using application.Interfaces;
using application.Models.Response.Player;
using csgo_stats.Models;

namespace application.Features.Mappers
{
    public class PlayerMapper : IPlayerMapper
    {
        public PlayerResponse MapPlayerResponse(string nickName, int totalGames, int winnedGames, int highestWinStreak, int mvp, int highestScore, string playerImage, string teamName) =>
            new PlayerResponse()
            {
                NickName = nickName,
                TotalGames = totalGames,
                WinnedGames = winnedGames,
                HighestWinStreak = highestWinStreak,
                MVPs = mvp,
                HighestScore = highestScore,
                PlayerImage = playerImage,
                TeamName = teamName
            };

        public RankingResponse MapRankingResponse(Player player, int score, Team team, int position) =>
            new RankingResponse()
            {
                RankingPosition = position,
                NickName = player.NickName,
                PlayerImage = player.Image,
                TeamLogo = team.Logo,
                TeamName = team.Name,
                TotalKills = score
            };
    }
}