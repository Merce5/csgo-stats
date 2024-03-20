namespace application.Models.Request
{
    public record CreateUserRequest(string userName, string password, string firstName, string lastName);
}