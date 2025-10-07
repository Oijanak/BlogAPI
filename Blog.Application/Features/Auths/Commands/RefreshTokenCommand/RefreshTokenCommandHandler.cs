using System.Net;
using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.Features.Auths.Commands.RefreshTokenCommand;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<TokenResponse>>
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<User> _userManager;

    public RefreshTokenCommandHandler(ITokenService tokenService, UserManager<User> userManager)
    {
        _tokenService = tokenService;
        _userManager = userManager;
    }

    public async Task<Result<TokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        var username = principal?.Identity?.Name;

        if (string.IsNullOrEmpty(username))
            return Result<TokenResponse>.Failure("Invalid access token", (int)HttpStatusCode.Unauthorized);

        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.UserName == username && u.RefreshToken == request.RefreshToken, cancellationToken);

        if (user == null)
            return Result<TokenResponse>.Failure("Invalid refresh token", (int)HttpStatusCode.Unauthorized);

        if (user.RefreshTokenExpires <= DateTime.UtcNow)
            return Result<TokenResponse>.Failure("Refresh token expired, please login again.", (int)HttpStatusCode.Unauthorized);

        var newAccessToken = _tokenService.GenerateToken(principal.Claims);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpires = DateTime.UtcNow.AddDays(7);

        await _userManager.UpdateAsync(user);

        var response = new TokenResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            RefreshTokenExpires = user.RefreshTokenExpires
        };

        return Result<TokenResponse>.Success(response, (int)HttpStatusCode.OK);
    }
}
