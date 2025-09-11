using BlogApi.Application.DTOs;
using MediatR;

namespace BlogApi.Application.SP.Authors.Commands.DeleteAuthorWithSpCommand;

public class DeleteAuthorWithSpCommand:IRequest<ApiResponse<string>>
{
    public Guid AuthorId { get; }

    public DeleteAuthorWithSpCommand(Guid authorId)
    {
        AuthorId = authorId;
    }
}