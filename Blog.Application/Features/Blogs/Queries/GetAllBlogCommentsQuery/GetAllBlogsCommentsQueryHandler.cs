using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.Features.Blogs.Queries.GetAllBlogCommentsQuery;

public class GetAllBlogsCommentsQueryHandler:IRequestHandler<GetAllBlogCommentsQuery,ApiResponse<IEnumerable<CommentDtos>>>
{
    private readonly IBlogDbContext _blogDbContext;
    private readonly ICurrentUserService _currentUserService;

    public GetAllBlogsCommentsQueryHandler(IBlogDbContext blogDbContext,ICurrentUserService currentUserService)
    {
        _blogDbContext = blogDbContext;
        _currentUserService = currentUserService;
    }
    public async Task<ApiResponse<IEnumerable<CommentDtos>>> Handle(GetAllBlogCommentsQuery request, CancellationToken cancellationToken)
    {
        var userId=_currentUserService.UserId;
        var comments = await _blogDbContext.Comments
            .Where(c => c.BlogId == request.BlogId && c.ParentCommentId == null)
            .Include(c => c.User)
            .Include(c => c.Replies)
            .ThenInclude(r => r.User)
            .Select(c => new CommentDtos
            {
                CommentId = c.CommentId,
                Content = c.Content,
                User = new UserDtos
                {
                    Id = c.User.Id,
                    Name = c.User.Name,
                    Email = c.User.Email
                },
                CreatedAt = c.CreatedAt,
                
                LikesCount = c.Reactions.Count(r => r.IsLike),
                DislikesCount = c.Reactions.Count(r => !r.IsLike),
                CurrentUserReaction = c.Reactions
                    .Where(r => r.UserId == userId)
                    .Select(r => (bool?)r.IsLike)
                    .FirstOrDefault(),
                Replies = c.Replies.Select(r => new CommentDtos
                {
                    CommentId = r.CommentId,
                    Content = r.Content,
                    User = new UserDtos
                    {
                        Id = r.User.Id,
                        Name = r.User.Name,
                        Email = r.User.Email
                    },
                    CreatedAt = c.CreatedAt,
                    LikesCount = r.Reactions.Count(re => re.IsLike),
                    DislikesCount = r.Reactions.Count(re => !re.IsLike),
                    CurrentUserReaction = r.Reactions
                        .Where(cr => cr.UserId == userId)
                        .Select(cr => (bool?)cr.IsLike)
                        .FirstOrDefault(),
                    
                }).ToList()
            })
            .ToListAsync();
        return new ApiResponse<IEnumerable<CommentDtos>>
        {
            Data = comments,
            Message = "Comments retrieved successfully",
        };
    }
}