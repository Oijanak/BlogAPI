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
        var blogDtos = await _blogDbContext.Blogs
            .Where(blog => blog.AuthorId == request.AuthorId)
            .Select(blog => new BlogDTO
            {
                BlogId = blog.BlogId,
                BlogTitle = blog.BlogTitle,
                BlogContent = blog.BlogContent,
                CreatedAt = blog.CreatedAt,
                UpdatedAt = blog.UpdatedAt

            }).ToListAsync();

       
        return new ApiResponse<IEnumerable<BlogDTO>>
        {
            Data = blogDtos,
            Message = "Authors blogs fetched successfully"
        };

    }
}