using BlogApi.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Application.Features.Comments.UpdateCommentCommand;

public class UpdateCommentCommand:IRequest<ApiResponse<CommentDto>>
{
    [FromRoute]
    public Guid CommentId { get; set; }
    [FromBody]
    public string Content { get; set; }
    
}