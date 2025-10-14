using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.Features.Comments.UpdateCommentCommand;

public class UpdateCommentCommandHandler:IRequestHandler<UpdateCommentCommand, ApiResponse<CommentDto>>
{
    private readonly IBlogDbContext _blogDbContext;
    private readonly ICurrentUserService _currentUserService;

    public UpdateCommentCommandHandler(IBlogDbContext blogDbContext,ICurrentUserService currentUserService)
    {
        _blogDbContext = blogDbContext;
        _currentUserService = currentUserService;
    }
    public async Task<ApiResponse<CommentDto>> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        var existingComment = await _blogDbContext.Comments
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.CommentId == request.CommentId, cancellationToken);
        
        existingComment.Content = request.Content;
        existingComment.UpdatedAt = DateTime.UtcNow;
        await _blogDbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse<CommentDto>
        {
            Data = new CommentDto
            {
                CommentId = existingComment.CommentId,
                Content = existingComment.Content,
                CreatedAt = existingComment.CreatedAt,
                UpdatedAt = existingComment.UpdatedAt,
                User = new UserDto
                {
                    Id = existingComment.User.Id,
                    Email = existingComment.User.Email,
                    Name = existingComment.User.Name,
                }
            },
            Message = "Comment updated successully"
        };

    }
}