using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.Features.Blogs.Queries.GetAllBlogCommentsQuery;

public class GetAllBlogsCommentsQueryHandler:IRequestHandler<GetAllBlogCommentsQuery,ApiResponse<IEnumerable<CommentDto>>>
{
    private readonly IBlogDbContext _blogDbContext;

    public GetAllBlogsCommentsQueryHandler(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<ApiResponse<IEnumerable<CommentDto>>> Handle(GetAllBlogCommentsQuery request, CancellationToken cancellationToken)
    {
        var comments = await _blogDbContext.Comments.Where(x => x.BlogId == request.BlogId)
            .Include(c=>c.User)
            .Select(comment => new CommentDto
            {
                CommentId = comment.CommentId,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt,
                User = new UserDto
                {
                    Id =  comment.User.Id,
                    Email = comment.User.Email,
                    Name = comment.User.Name
                }

            }).ToListAsync();
        return new ApiResponse<IEnumerable<CommentDto>>
        {
            Data = comments,
            Message = "Comments retrieved successfully",
        };
    }
}