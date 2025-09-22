using BlogApi.Application.DTOs;
using MediatR;

namespace BlogApi.Application.Features.Auths.Commands.RefreshTokenCommand;

public class RefreshTokenCommand:IRequest<TokenResponse>
{
    public string AccessToken { get; set; } 
    public string RefreshToken { get;set; } 
}