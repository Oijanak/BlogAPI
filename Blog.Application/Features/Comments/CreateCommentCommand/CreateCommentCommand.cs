using BlogApi.Application.DTOs;
using MediatR;

namespace BlogApi.Application.Features.Comments.CreateCommentCommand;

public class CreateCommentCommand:IRequest<ApiResponse<CommentDto>>
{
    public Guid BlogId { get; set; }
    public string Content { get; set; }
    
}