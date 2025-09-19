using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace BlogApi.Infrastructure.Services;


public class TokenCleanupService : ITokenCleanupService
{
    private readonly UserManager<User> _userManager;

    public TokenCleanupService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task RemoveExpiredTokensAsync()
    {
        var users = _userManager.Users
            .Where(u => u.RefreshTokenExpires.HasValue && u.RefreshTokenExpires < DateTime.UtcNow)
            .ToList<User>();

        foreach (var user in users)
        {
            user.RefreshToken = null;
            user.RefreshTokenExpires = null;
            await _userManager.UpdateAsync(user);
        }
    }
}
