using BlogApi.Application.DTOs;
using MediatR;

namespace BlogApi.Application.SP.Users.Commands.DeleteUserWithSpCommand;

public class DeleteUserWithSpCommand:IRequest<ApiResponse<string>>
{
    public Guid UserId { get; }

    public DeleteUserWithSpCommand(Guid userId)
    {
        UserId = userId;
    }
    
}