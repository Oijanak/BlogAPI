using BlogApi.Application.DTOs;
using MediatR;

namespace BlogApi.Application.Features.Auths.Commands.LoginUserCommand;

public class LoginUserCommand:IRequest<Result<TokenResponse>>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}