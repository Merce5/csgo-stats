using domain.Models.Auth;

namespace application.Interfaces
{
    public interface IUserService
    {
        Task<bool> CheckPasswordAsync(User user, string password);
        Task CreateUser(User newUser, string password);
        Task<User> FindByNameAsync(string userName);
        User? GetById(int id);
    }
}