namespace application.Interfaces
{
    public interface IJwtUtils
    {
        int? ValidateToken(string token);
    }
}