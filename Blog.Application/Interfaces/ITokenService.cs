namespace BlogApi.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(int userId, string email);
    }
}