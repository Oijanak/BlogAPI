using System.Security.Claims;

namespace BlogApi.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(List<Claim> claims);
        string GenerateRefreshToken();
    }
}