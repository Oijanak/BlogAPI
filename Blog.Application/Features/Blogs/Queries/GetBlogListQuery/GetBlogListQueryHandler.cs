using BlogApi.Application.DTOs;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.Features.Blogs.Queries.GetBlogListQuery;

public class GetBlogListQueryHandler:IRequestHandler<GetBlogListQuery, ApiResponse<IEnumerable<BlogDTO>>>
{
    private readonly IBlogDbContext _blogDbContext;

    public GetBlogListQueryHandler(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    
    public async Task<ApiResponse<IEnumerable<BlogDTO>>> Handle(GetBlogListQuery request, CancellationToken cancellationToken)
    {
        var blogsEntities = await _blogDbContext.Blogs
            .Include(blog => blog.Author)
            .ToListAsync();

        var blogDTOs = blogsEntities.Select(blog => new BlogDTO
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
            Data = blogDTOs,
            Message = "Blogs fetched successfully"
        };
    }
}