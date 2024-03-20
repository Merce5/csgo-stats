using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using application.Interfaces;
using application.Models.Request;
using application.Models.Settings;
using application.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using domain.Models.Auth;

namespace csgo_stats.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly CsgoDbContext _context;
        private readonly IOptions<JwtSettings> _jwtSettings;
        public TokenController(IUserService userService, CsgoDbContext context, IOptions<JwtSettings> jwtSettings)
        {
            _userService = userService;
            _context = context;
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

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}")
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Value.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(
                issuer: _jwtSettings.Value.Issuer,
                audience: _jwtSettings.Value.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(720),
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

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