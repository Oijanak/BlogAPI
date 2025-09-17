using BlogApi.Application.DTOs;
using MediatR;

namespace BlogApi.Application.Features.Users.Commands.LoginUserCommand;

public class LoginUserCommand:IRequest<TokenResponse>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}