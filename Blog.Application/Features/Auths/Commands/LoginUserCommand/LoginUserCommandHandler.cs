using System.Net;
using System.Security.Claims;
using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace BlogApi.Application.Features.Auths.Commands.LoginUserCommand;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<TokenResponse>>
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;
    private readonly SignInManager<User> _signInManager;

    public LoginUserCommandHandler(UserManager<User> userManager,SignInManager<User> signInManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _signInManager = signInManager;
    }

    public async Task<Result<TokenResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return Result<TokenResponse>.Failure("Invalid email or password", (int)HttpStatusCode.Unauthorized);

        var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, lockoutOnFailure: true);

        if (result.IsLockedOut)
            return Result<TokenResponse>.Failure("Account locked due to multiple failed attempts.", (int)HttpStatusCode.Locked);

        if (result.IsNotAllowed)
            return Result<TokenResponse>.Failure("Please confirm your email before logging in.", (int)HttpStatusCode.Unauthorized);

        if (!result.Succeeded)
            return Result<TokenResponse>.Failure("Invalid email or password", (int)HttpStatusCode.Unauthorized);
        var userRoles = await _userManager.GetRolesAsync(user);
        var authClaims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

        var accessToken = _tokenService.GenerateToken(authClaims);
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpires = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);

        var response = new TokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            RefreshTokenExpires = user.RefreshTokenExpires
        };

        return Result<TokenResponse>.Success(response, (int)HttpStatusCode.OK);
    }
    
    
}
