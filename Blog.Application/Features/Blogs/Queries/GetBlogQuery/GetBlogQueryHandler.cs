using BlogApi.Application.DTOs;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.Features.Blogs.Queries.GetBlogQuery;

public class GetBlogQueryHandler:IRequestHandler<GetBlogQuery,ApiResponse<BlogDTO>>
{
    private readonly IBlogDbContext _blogDbContext;

    public GetBlogQueryHandler(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    
    public async Task<ApiResponse<BlogDTO>> Handle(GetBlogQuery request, CancellationToken cancellationToken)
    {
        Blog blog = await _blogDbContext.Blogs
            .Include(b => b.Author)
            .FirstOrDefaultAsync(b => b.BlogId == request.BlogId, cancellationToken);
        var blogResult= new BlogDTO()
        {
            BlogId = blog.BlogId,
            BlogTitle = blog.BlogTitle,
            BlogContent = blog.BlogContent,
            CreatedAt = blog.CreatedAt,
            UpdatedAt = blog.UpdatedAt,
            CreatedBy=blog.CreatedBy,
            UpdatedBy = blog.UpdatedBy,
            StartDate = blog.StartDate,
            EndDate = blog.EndDate,
            ApprovedBy = blog.ApprovedBy,
            ApproveStatus = blog.ApproveStatus,
            ActiveStatus = blog.ActiveStatus,
            Author = new AuthorDto(blog.Author)
        };
        return new ApiResponse<BlogDTO>
        {
            Data = blogResult,
            Message = "Blog fetched successfully"
        };
    }
}