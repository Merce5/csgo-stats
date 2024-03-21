namespace application.Interfaces
{
    public interface IJwtUtils
    {
        string ValidateToken(string token);
    }
}