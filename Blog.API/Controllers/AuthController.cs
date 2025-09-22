using BlogApi.Application.Features.Auths.Commands.LoginUserCommand;
using BlogApi.Application.Features.Auths.Commands.RefreshTokenCommand;
using BlogApi.Application.Features.Auths.Commands.RegisterUserCommand;
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
        return StatusCode(StatusCodes.Status201Created,await _sender.Send(request));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserCommand request)
    {
        return Ok(await _sender.Send(request));
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenCommand request)
    {
        return Ok(await _sender.Send(request));
    }

}