using BlogApi.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Application.Features.Users.Commands.DeleteUserCommand;

public class DeleteUserCommand:IRequest<ApiResponse<string>>
{
    [FromRoute]
    public Guid UserId { get; set; }

    public DeleteUserCommand(){}
    public DeleteUserCommand(Guid userId)
    {
        UserId = userId;
    }
}