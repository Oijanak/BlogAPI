using BlogApi.Application.Features.Auths.Commands.ConfirmEmailCommand;
using BlogApi.Application.Features.Auths.Commands.ForgetPasswordCommand;
using BlogApi.Application.Features.Auths.Commands.LoginUserCommand;
using BlogApi.Application.Features.Auths.Commands.RefreshTokenCommand;
using BlogApi.Application.Features.Auths.Commands.RegisterUserCommand;
using BlogApi.Application.Features.Auths.Commands.ResetPasswordCommand;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BlogApi.Application.DTOs;
namespace BlogApi.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController:ControllerBase
{
    private readonly ISender _sender;
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;

    public AuthController(ISender sender,UserManager<User> userManager,ITokenService tokenService)
    {
        _sender = sender;
        _userManager = userManager;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserCommand request)
    {
        var result = await _sender.Send(request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserCommand request)
    {
        var result = await _sender.Send(request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenCommand request)
    {
        var result = await _sender.Send(request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(ConfirmEmailCommand request)
    {
        var result = await _sender.Send(request);
        return StatusCode(result.StatusCode, result);
    }
    
    
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordCommand command)
    {
        var result = await _sender.Send(command);
        return StatusCode(result.StatusCode, result);
    }
    
    [HttpPost("reset-password")]
    
    public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
    {
        var result = await _sender.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("google-login")]
    public async Task<IActionResult> GoogleLogin([FromBody] string idToken)
    {
        // Validate ID token
        var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);

        // Create or get user
        var user = await _userManager.FindByEmailAsync(payload.Email);
        if (user == null)
        {
            user = new User
            {
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

        return Ok(response);
    }

}