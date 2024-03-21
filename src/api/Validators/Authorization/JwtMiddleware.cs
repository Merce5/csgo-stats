using application.Interfaces;

namespace csgo_stats.api.Validators.Authorization
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IUserService _userService;
        private readonly IJwtUtils _jwtUtils;

        public JwtMiddleware(RequestDelegate next, IUserService userService, IJwtUtils jwtUtils)
        {
            _next = next;
            _userService = userService;
            _jwtUtils = jwtUtils;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtUtils.ValidateToken(token!);
            if (userId != null)
            {
                context.Items["User"] = _userService.GetById(userId);
            }

            await _next(context);
        }
    }
}