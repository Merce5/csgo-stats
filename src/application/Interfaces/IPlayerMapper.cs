using application.Models.Response.Player;
using csgo_stats.Models;

namespace application.Interfaces
{
    public interface IPlayerMapper
    {
        PlayerResponse MapPlayerResponse(string nickName, int totalGames, int winnedGames, int highestWinStreak, int mvp, int highestScore, string playerImage, string teamName);
        RankingResponse MapRankingResponse(Player player, int score, Team team, int position);
    }
}