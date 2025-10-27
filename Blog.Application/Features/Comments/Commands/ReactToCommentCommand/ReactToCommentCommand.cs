using BlogApi.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Application.Features.Comments.Commands.ReactToCommentCommand;

public class ReactToCommentCommand:IRequest<Result<ReactToCommentDto>>
{
    [FromRoute]
    public Guid CommentId { get; set; }
    [FromBody]
    public bool IsLike { get; set; } 
}