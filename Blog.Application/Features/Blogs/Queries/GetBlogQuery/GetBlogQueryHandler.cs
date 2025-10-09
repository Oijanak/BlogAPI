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
            .Include(b => b.CreatedByUser)
            .Include(b => b.UpdatedByUser)
            .Include(b => b.ApprovedByUser)
            .Include(b=>b.Categories)
            .FirstOrDefaultAsync(b => b.BlogId == request.BlogId, cancellationToken);
        var blogResult = new BlogDTO()
        {
            BlogId = blog.BlogId,
            BlogTitle = blog.BlogTitle,
            BlogContent = blog.BlogContent,
            CreatedAt = blog.CreatedAt,
            UpdatedAt = blog.UpdatedAt,
            CreatedBy = blog.CreatedByUser != null ? new UserDto(blog.CreatedByUser) : null,
            UpdatedBy = blog.UpdatedByUser != null ? new UserDto(blog.UpdatedByUser) : null,
            ApprovedBy = blog.ApprovedByUser != null ? new UserDto(blog.ApprovedByUser) : null,
            StartDate = blog.StartDate,
            EndDate = blog.EndDate,
            ApproveStatus = blog.ApproveStatus,
            ActiveStatus = blog.ActiveStatus,
            Author = blog.Author != null ? new AuthorDto(blog.Author) : null,
            Categories = blog.Categories.Select(c=>new CategoryDto{CategotyId= c.CategoryId,CategoryName = c.CategoryName}).ToList()
            
        };
        return new ApiResponse<BlogDTO>
        {
            Data = blogResult,
            Message = "Blog fetched successfully"
        };
    }
}