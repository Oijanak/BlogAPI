using BlogApi.Application.DTOs;
using BlogApi.Application.Features.Auths.Commands.LoginWithGoolgeCommand;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Google.Apis.Auth.GoogleJsonWebSignature;
namespace BlogApi.Application.Features.Auths.Commands.LoginWithGoolgeCommand
{
    public class LoginWithGoogleCommandHandler : IRequestHandler<LoginWithGoogleCommand, Result<TokenResponse>>
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;
        public LoginWithGoogleCommandHandler(UserManager<User> userManager, ITokenService tokenService, IConfiguration config)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _config = config;
        }
        public async Task<Result<TokenResponse>> Handle(LoginWithGoogleCommand request, CancellationToken cancellationToken)
        {
            Payload payload = new();


            payload = await ValidateAsync(request.IdToken, new ValidationSettings
            {
                Audience = new[] { _config["Google:ClientId"] }
            });
            

            var user = await _userManager.FindByEmailAsync(payload.Email);
            if (user == null)
            {
                user = new User
                {
                    Name = payload.Name,
                    UserName = payload.Email,
                    Email = payload.Email,
                    EmailConfirmed = true
                };
                await _userManager.CreateAsync(user);
                await _userManager.AddToRoleAsync(user, "User");
            }


            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
        {
            new(ClaimTypes.PrimarySid, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
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

            return Result<TokenResponse>.Success(response);
        }
    }
}
