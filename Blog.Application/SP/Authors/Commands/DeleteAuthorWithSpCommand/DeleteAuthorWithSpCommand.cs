using BlogApi.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Application.SP.Authors.Commands.DeleteAuthorWithSpCommand;

public class DeleteAuthorWithSpCommand:IRequest<ApiResponse<string>>
{
    [FromRoute]
    public Guid AuthorId { get; set; }
    public DeleteAuthorWithSpCommand(Guid authorId)
    {
        AuthorId = authorId;
    }
}