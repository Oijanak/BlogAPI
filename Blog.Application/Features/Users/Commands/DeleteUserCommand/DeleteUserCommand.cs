using BlogApi.Application.DTOs;
using MediatR;

namespace BlogApi.Application.Features.Users.Commands.DeleteUserCommand;

public class DeleteUserCommand:IRequest<ApiResponse<string>>
{
    public Guid UserId { get; }

    public DeleteUserCommand(Guid userId)
    {
        UserId = userId;
    }
}