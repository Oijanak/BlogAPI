using BlogApi.Application.DTOs;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.Features.Authors.Queries.GetBlogsByAuthorIdQuery;

public class GetBlogsAuthorIdQueryHandler:IRequestHandler<GetBlogsByAuthorIdQuery,IEnumerable<BlogDTO>>
{
    private readonly BlogDbContext _blogDbContext;

    public GetBlogsAuthorIdQueryHandler(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<IEnumerable<BlogDTO>> Handle(GetBlogsByAuthorIdQuery request, CancellationToken cancellationToken)
    {
        return await _blogDbContext.Blogs
            .Where(blog => blog.AuthorId == request.AuthorId)
            .Include(blog => blog.Author) 
            .Select(blog => new BlogDTO
            {
                BlogId = blog.BlogId,
                BlogTitle = blog.BlogTitle,
                BlogContent = blog.BlogContent,
                CreatedAt = blog.CreatedAt,
                UpdatedAt = blog.UpdatedAt,
                Author = new AuthorDTO(blog.Author)
            })
            .ToListAsync();
    }
}