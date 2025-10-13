using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.Features.Comments.CreateCommentCommand;

public class CreateCommentCommandHandler:IRequestHandler<CreateCommentCommand, ApiResponse<CommentDto>>
{
    private readonly IBlogDbContext  _blogDbContext;
    private readonly ICurrentUserService _currentUserService;

    public CreateCommentCommandHandler(IBlogDbContext blogDbContext, ICurrentUserService currentUserService)
    {
        _blogDbContext = blogDbContext;
        _currentUserService = currentUserService;
    }
    public async Task<ApiResponse<CommentDto>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var user = await _blogDbContext.Users
            .Where(u => u.Id == _currentUserService.UserId)
            .Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                Name = u.Name
            })
            .FirstOrDefaultAsync(cancellationToken);
        var comment = new Comment
        {
            Content = request.Content,
            BlogId = request.BlogId,
            UserId = _currentUserService.UserId
        };
        _blogDbContext.Comments.Add(comment);
        await _blogDbContext.SaveChangesAsync();
        return new ApiResponse<CommentDto>
        {
            Data = new CommentDto
            {
                CommentId = comment.CommentId,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt,
                User = user

            },
            Message = "Comment created successfully"
        };
    }
}