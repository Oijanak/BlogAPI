using BlogApi.Application.DTOs;
using MediatR;

namespace BlogApi.Application.Features.Authors.Commands.DeleteAuthorCommand;

public class DeleteAuthorCommand:IRequest<ApiResponse<string>>
{
    public Guid AuthorId { get; }

    public DeleteAuthorCommand(Guid authorId)
    {
        AuthorId = authorId;
    }
}