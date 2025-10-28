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
    private readonly ICurrentUserService _currentUserService;
    public GetBlogQueryHandler(IBlogDbContext blogDbContext,ICurrentUserService currentUserService)
    {
        _blogDbContext = blogDbContext;
        _currentUserService = currentUserService;
    }
    
    public async Task<ApiResponse<BlogDTO>> Handle(GetBlogQuery request, CancellationToken cancellationToken)
    {
        Blog blog = await _blogDbContext.Blogs
             .Include(b => b.Author)
             .ThenInclude(a=>a.Followers)
            .Include(b => b.CreatedByUser)
            .Include(b => b.UpdatedByUser)
            .Include(b => b.ApprovedByUser)
            .Include(b=>b.Categories)
            .Include(b=>b.Documents)
            .Include(b=>b.FavoritedBy)
            .FirstOrDefaultAsync(b => b.BlogId == request.BlogId, cancellationToken);
         var userId = _currentUserService.UserId;
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
            Author = blog.Author != null ? new AuthorDto()
            {
                AuthorId = blog.Author.AuthorId,
                AuthorName = blog.Author.AuthorName,
                AuthorEmail = blog.Author.AuthorEmail,
                Age = blog.Author.Age,
                CreatedBy = blog.Author.CreatedBy,
                isFollowed = userId!=null?blog.Author.Followers.Any(f=>f.UserId==userId):null
            } : null,
            Categories = blog.Categories.Select(c=>new CategoryDto{CategotyId= c.CategoryId,CategoryName = c.CategoryName}).ToList(),
            BlogDocuments = blog.Documents.Select(d=>new BlogDocumentDto{BlogDocumentId = d.BlogDocumentId,DocumentName = d.DocumentName,DocumentType = d.DocumentType}).ToList(),
            isFavorited = userId!=null?blog.FavoritedBy.Any(bf => bf.UserId == userId):null
        };
        return new ApiResponse<BlogDTO>
        {
            Data = blogResult,
            Message = "Blog fetched successfully"
        };
    }
}