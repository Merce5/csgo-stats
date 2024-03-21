using System.Text;
using application.Features.Authorization;
using application.Features.Queries;
using application.Interfaces;
using application.Models.Settings;
using domain.Models.Auth;
using application.Features;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using application.Features.Mappers;

namespace csgo_stats.api
{
    public static class Startup
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            services.Configure<JwtSettings>(configuration.GetSection("Jwt"));

            services.AddDbServices(configuration);
            services.AddIdentityServices();
            services.AddJwtServices(configuration);
            services.AddWebServices();
            services.AddCustomServices();

            return services;
        }

        private static IServiceCollection AddDbServices(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContext<CsgoDbContext>(op =>
                op.UseSqlServer(configuration.GetConnectionString("DefaultValue"))
            );

        private static IdentityBuilder AddIdentityServices(this IServiceCollection services) =>
            services.AddIdentityCore<User>()
                .AddEntityFrameworkStores<CsgoDbContext>();

        private static AuthenticationBuilder AddJwtServices(this IServiceCollection services, IConfiguration configuration) =>
            services.AddHttpContextAccessor()
                .AddAuthorization()
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(op =>
                    {
                        op.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = configuration["Jwt:Issuer"],
                            ValidAudience = configuration["Jwt:Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                        };
                    }
                );

        private static IMvcBuilder AddWebServices(this IServiceCollection services) =>
            services.AddEndpointsApiExplorer()
                .AddControllers();

        private static IServiceCollection AddCustomServices(this IServiceCollection services) =>
            services
                .AddSingleton<IUserService, UserService>()
                .AddSingleton<IPlayerService, PlayerService>()
                .AddSingleton<IPlayerMapper, PlayerMapper>()
                .AddSingleton<IJwtUtils, JwtUtils>();
    }
}