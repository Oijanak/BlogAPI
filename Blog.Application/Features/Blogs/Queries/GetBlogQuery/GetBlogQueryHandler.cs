using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using BlogApi.Infrastructure.Data;
using MediatR;

namespace BlogApi.Application.Features.Blogs.Queries.GetBlogQuery;

public class GetBlogQueryHandler:IRequestHandler<GetBlogQuery,BlogDTO>
{
    private readonly BlogDbContext _blogDbContext;

    public GetBlogQueryHandler(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    
    public async Task<BlogDTO> Handle(GetBlogQuery request, CancellationToken cancellationToken)
    {
        Blog blog = await _blogDbContext.Blogs.FindAsync(request.BlogId) ?? throw new Exception("Blog not found");
        return new BlogDTO()
        {
            BlogId = blog.BlogId,
            BlogTitle = blog.BlogTitle,
            BlogContent = blog.BlogContent,
            CreatedAt = blog.CreatedAt,
            UpdatedAt = blog.UpdatedAt,
        };
    }
}