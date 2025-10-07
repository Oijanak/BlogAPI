using BlogApi.Application.DTOs;
using MediatR;

namespace BlogApi.Application.Features.Auths.Commands.RefreshTokenCommand;

public class RefreshTokenCommand:IRequest<Result<TokenResponse>>
{
    public string AccessToken { get; set; } 
    public string RefreshToken { get;set; } 
}