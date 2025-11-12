using BlogApi.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Application.Features.Comments.UpdateCommentCommand;

public class UpdateCommentCommand:IRequest<ApiResponse<CommentDtos>>
{
    [FromRoute]
    public Guid CommentId { get; set; }
    [FromBody]
    public UpdateCommentRequest UpdateCommentRequest { get; set; }
    
}

public class UpdateCommentRequest
{
    public string Content { get; set; }
}