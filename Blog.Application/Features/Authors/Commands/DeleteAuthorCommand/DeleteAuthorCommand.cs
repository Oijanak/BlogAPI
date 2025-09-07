using MediatR;

namespace BlogApi.Application.Features.Authors.Commands.DeleteAuthorCommand;

public class DeleteAuthorCommand:IRequest<Unit>
{
    public Guid AuthorId { get; }

    public DeleteAuthorCommand(Guid authorId)
    {
        AuthorId = authorId;
    }
}