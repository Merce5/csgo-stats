using application.Interfaces;
using application.Models.Request;
using application.Models.Settings;
using domain.Models.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace csgo_stats.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IOptions<JwtSettings> _jwtSettings;
        public TokenController(IUserService userService, IOptions<JwtSettings> jwtSettings)
        {
            _userService = userService;
            _jwtSettings = jwtSettings;
        }

        [HttpPost]
        public async Task<IResult> GetToken([FromBody] TokenRequest request)
        {
            var user = await _userService.FindByNameAsync(request.UserName);

            if (user is null || !await _userService.CheckPasswordAsync(user, request.Password))
            {
                return Results.Forbid();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Value.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(20),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return Results.Ok(new
            {
                AccessToken = jwt
            });
        }

        [HttpPost("create")]
        public async Task<IResult> CreateUser([FromBody] CreateUserRequest request)
        {
            var user = await _userService.FindByNameAsync(request.userName);


            if (user is not null)
            {
                return Results.BadRequest("User name already exists");
            }

            var newUser = new User()
            {
                UserName = request.userName,
                FirstName = request.firstName,
                LastName = request.lastName
            };

            await _userService.CreateUser(newUser, request.password);

            return Results.Ok();
        }
    }
}