using BlogApi.Application.DTOs;
using MediatR;

namespace BlogApi.Application.Features.Comments.CreateCommentCommand;

public class CreateCommentCommand:IRequest<ApiResponse<CommentDtos>>
{
    public Guid BlogId { get; set; }
    public string Content { get; set; }
    
    public Guid? ParentCommentId { get; set; }
    
}