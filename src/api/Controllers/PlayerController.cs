using application.Interfaces;
using application.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace csgo_stats.api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _playerService;
        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }
        [HttpGet]
        public IResult GetOnePlayer([FromQuery] int? id, [FromQuery] string? nickName)
        {
            if (HttpContext.Items["User"] == null)
            {
                return Results.Conflict("Unauthorized 404");
            }

            PlayerResponse? response = null;
            if (id != null && nickName != null)
            {
                return Results.Conflict(new
                {
                    Error = "Choose only one filter value"
                });
            }
            else if (id != null)
            {
                response = _playerService.GetPlayerById(id);
            }
            else
            {
                response = _playerService.GetPlayerByName(nickName);
            }
            return Results.Ok(response);
        }

        [HttpGet("ranking")]
        public IResult GetRanking()
        {
            if (HttpContext.Items["User"] == null)
            {
                return Results.Conflict("Unauthorized 404");
            }
            IEnumerable<RankingResponse>? response = _playerService.GetTopPlayersStats();
            return Results.Ok(response);
        }
    }
}