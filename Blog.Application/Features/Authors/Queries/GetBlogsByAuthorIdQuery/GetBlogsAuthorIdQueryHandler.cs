using BlogApi.Application.DTOs;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.Features.Authors.Queries.GetBlogsByAuthorIdQuery;

public class GetBlogsAuthorIdQueryHandler:IRequestHandler<GetBlogsByAuthorIdQuery,ApiResponse<IEnumerable<BlogDTO>>>
{
    private readonly IBlogDbContext _blogDbContext;

    public GetBlogsAuthorIdQueryHandler(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<ApiResponse<IEnumerable<BlogDTO>>> Handle(GetBlogsByAuthorIdQuery request, CancellationToken cancellationToken)
    {
        var blogsEntities = await _blogDbContext.Blogs
            .Where(blog => blog.AuthorId == request.AuthorId)
            .Include(blog => blog.Author)
            .ToListAsync(cancellationToken);

        var blogs = blogsEntities.Select(blog => new BlogDTO
        {
            BlogId = blog.BlogId,
            BlogTitle = blog.BlogTitle,
            BlogContent = blog.BlogContent,
            CreatedAt = blog.CreatedAt,
            UpdatedAt = blog.UpdatedAt,
            Author = new AuthorDto(blog.Author) 
        }).ToList();
        return new ApiResponse<IEnumerable<BlogDTO>>
        {
            Data = blogs,
            Message = "Authors blogs fetched successfully"
        };

    }
}