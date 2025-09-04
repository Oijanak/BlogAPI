using MediatR;

namespace BlogApi.Application.Features.Users.Commands.DeleteUserCommand;

public class DeleteUserCommand:IRequest<Unit>
{
    public int UserId { get; }

    public DeleteUserCommand(int userId)
    {
        UserId = userId;
    }
}