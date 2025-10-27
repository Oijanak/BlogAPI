using BlogApi.Application.Features.Auths.Commands.ConfirmEmailCommand;
using BlogApi.Application.Features.Auths.Commands.ForgetPasswordCommand;
using BlogApi.Application.Features.Auths.Commands.LoginUserCommand;
using BlogApi.Application.Features.Auths.Commands.RefreshTokenCommand;
using BlogApi.Application.Features.Auths.Commands.RegisterUserCommand;
using BlogApi.Application.Features.Auths.Commands.ResetPasswordCommand;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController:ControllerBase
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
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

}