using application.Interfaces;
using domain.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace application.Features.Queries
{
    public class UserService : IUserService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IServiceProvider _serviceProvider;
        public UserService(IServiceScopeFactory serviceScopeFactory, IServiceProvider serviceProvider)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _serviceProvider = serviceProvider;
        }

        public User? GetById(string id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CsgoDbContext>();
                return dbContext.Users.Where(user => user.Id == id).FirstOrDefault();
            }
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                return await userManager.CheckPasswordAsync(user, password);
            }
        }

        public async Task<User> FindByNameAsync(string userName)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                return await userManager.FindByNameAsync(userName);
            }
        }

        public async Task CreateUser(User newUser, string password)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                await userManager.CreateAsync(newUser, password);
            }
        }
    }
}