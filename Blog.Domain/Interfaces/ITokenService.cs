namespace BlogApi.Domain.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(int userId, string email);
    }
}