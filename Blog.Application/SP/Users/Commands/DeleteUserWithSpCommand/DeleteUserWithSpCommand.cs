using MediatR;

namespace BlogApi.Application.SP.Users.Commands.DeleteUserWithSpCommand;

public class DeleteUserWithSpCommand:IRequest<Unit>
{
    public Guid UserId { get; }

    public DeleteUserWithSpCommand(Guid userId)
    {
        UserId = userId;
    }
    
}