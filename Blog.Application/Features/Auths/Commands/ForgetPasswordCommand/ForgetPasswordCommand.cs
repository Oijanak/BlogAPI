using BlogApi.Application.DTOs;
using MediatR;

namespace BlogApi.Application.Features.Auths.Commands.ForgetPasswordCommand;
public class ForgotPasswordCommand : IRequest<Result<string>>
{
    public string Email { get; set; }
}