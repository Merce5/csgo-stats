using application.Models.Response;

namespace application.Interfaces
{
    public interface IPlayerService
    {
        PlayerResponse? GetPlayerByName(string? nickName);
        PlayerResponse? GetPlayerById(int? id);
        IEnumerable<RankingResponse> GetTopPlayersStats();
    }
}