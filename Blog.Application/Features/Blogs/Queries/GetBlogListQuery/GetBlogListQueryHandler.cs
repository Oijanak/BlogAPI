using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using BlogApi.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.Features.Blogs.Queries.GetBlogListQuery;

public class GetBlogListQueryHandler:IRequestHandler<GetBlogListQuery, IEnumerable<BlogDTO>>
{
    private readonly BlogDbContext _blogDbContext;

    public GetBlogListQueryHandler(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    
    public async Task<IEnumerable<BlogDTO>> Handle(GetBlogListQuery request, CancellationToken cancellationToken)
    {
        return await _blogDbContext.Blogs.Select(blog => new BlogDTO()
        {
            BlogId = blog.BlogId,
            BlogTitle = blog.BlogTitle,
            BlogContent = blog.BlogContent,
            CreatedAt = blog.CreatedAt,
            UpdatedAt = blog.UpdatedAt,
        }).ToListAsync();
    }
}