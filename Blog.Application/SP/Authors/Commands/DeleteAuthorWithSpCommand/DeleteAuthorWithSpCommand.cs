using MediatR;

namespace BlogApi.Application.SP.Authors.Commands.DeleteAuthorWithSpCommand;

public class DeleteAuthorWithSpCommand:IRequest<Unit>
{
    public Guid AuthorId { get; }

    public DeleteAuthorWithSpCommand(Guid authorId)
    {
        AuthorId = authorId;
    }
}