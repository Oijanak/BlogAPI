using BlogApi.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Application.Features.Auths.Commands.ConfirmEmailCommand;

public class ConfirmEmailCommand:IRequest<Result<string>>
{
    
    public string UserId { get; set; }
    
    public string Token { get; set; }
}