using BlogApi.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Application.SP.Users.Commands.DeleteUserWithSpCommand;

public class DeleteUserWithSpCommand:IRequest<ApiResponse<string>>
{
    [FromRoute]
    public Guid UserId { get; }

    public DeleteUserWithSpCommand(Guid userId)
    {
        UserId = userId;
    }
    
}