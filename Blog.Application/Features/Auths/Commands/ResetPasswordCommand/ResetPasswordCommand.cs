using BlogApi.Application.DTOs;
using MediatR;

namespace BlogApi.Application.Features.Auths.Commands.ResetPasswordCommand;

public class ResetPasswordCommand : IRequest<Result<string>>
{
    public string UserId { get; set; }
    public string Token { get; set; }
    public string NewPassword { get; set; }
}