using application.Models.Response.Team;

namespace application.Interfaces
{
    public interface ITeamService
    {
        TeamResponse? GetTeamById(int? id);
        TeamResponse? GetTeamByName(string? name);
        IEnumerable<TeamRankingResponse> GetRankingTeamResponse();
    }
}