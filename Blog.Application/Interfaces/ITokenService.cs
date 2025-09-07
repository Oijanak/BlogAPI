namespace BlogApi.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(Guid userId, string email);
    }
}