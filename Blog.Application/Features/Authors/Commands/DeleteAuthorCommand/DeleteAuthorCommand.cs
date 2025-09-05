using MediatR;

namespace BlogApi.Application.Features.Authors.Commands.DeleteAuthorCommand;

public class DeleteAuthorCommand:IRequest<Unit>
{
    public int AuthorId { get; }

    public DeleteAuthorCommand(int authorId)
    {
        AuthorId = authorId;
    }
}