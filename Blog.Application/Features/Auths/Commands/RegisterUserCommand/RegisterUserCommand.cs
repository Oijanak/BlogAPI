
using BlogApi.Application.DTOs;
using BlogApi.Domain.Enum;
using MediatR;

namespace BlogApi.Application.Features.Auths.Commands.RegisterUserCommand;

public record RegisterUserCommand(string Name,string Email, string Password) 
    : IRequest<Result<string>>;