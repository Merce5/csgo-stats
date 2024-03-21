using application.Interfaces;
using application.Models.Response.Team;
using Microsoft.AspNetCore.Mvc;

namespace csgo_stats.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpGet]
        public IResult GetOneTeam([FromQuery] int? id, [FromQuery] string? name)
        {
            if (HttpContext.Items["User"] == null)
            {
                return Results.Conflict("Unauthorized 404");
            }

            if (id != null && name != null)
            {
                return Results.Conflict(new
                {
                    Error = "Choose only one filter value"
                });
            }
            else if (id != null)
            {
                return Results.Ok(_teamService.GetTeamById(id));
            }
            else
            {
                return Results.Ok(_teamService.GetTeamByName(name));
            }
        }

        [HttpGet]
        [Route("ranking")]
        public IResult GetRanking()
        {
            if (HttpContext.Items["User"] == null)
            {
                return Results.Conflict("Unauthorized 404");
            }
            IEnumerable<TeamRankingResponse>? response = _teamService.GetRankingTeamResponse();
            return Results.Ok(response);
        }
    }
}