using BlogApi.Application.DTOs;
using MediatR;

namespace BlogApi.Application.Features.Auths.Commands.RegisterUserCommand;

public record RegisterUserCommand(string Name,string Email, string Password) 
    : IRequest<ApiResponse<string>>;