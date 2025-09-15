using BlogApi.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Application.Features.Authors.Commands.DeleteAuthorCommand;

public class DeleteAuthorCommand:IRequest<ApiResponse<string>>
{
    [FromRoute]
    public Guid AuthorId { get; set; }
    public DeleteAuthorCommand(){}

    public DeleteAuthorCommand(Guid authorId)
    {
        AuthorId = authorId;
    }
}