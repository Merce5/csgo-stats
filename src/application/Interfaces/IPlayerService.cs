using application.Models.Response.Player;

namespace application.Interfaces
{
    public interface IPlayerService
    {
        PlayerResponse? GetPlayerByName(string? nickName);
        PlayerResponse? GetPlayerById(int? id);
        IEnumerable<RankingResponse> GetTopPlayersStats();
        IEnumerable<PlayerResponse> GetAllPlayers();
    }
}