using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.Features.BlogFavorite.Queries.GetFavoriteBlogsQuery;

public class GetFavoriteBlogsQueryHandler:IRequestHandler<GetFavoriteBlogsQuery, Result<IEnumerable<BlogDTO>>>
{
    private readonly IBlogDbContext _blogDbContext;
    private readonly ICurrentUserService _currentUserService;

    public GetFavoriteBlogsQueryHandler(IBlogDbContext blogDbContext, ICurrentUserService currentUserService)
    {
        _blogDbContext = blogDbContext;
        _currentUserService = currentUserService;
    }
    public async Task<Result<IEnumerable<BlogDTO>>> Handle(GetFavoriteBlogsQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        var blogs= await _blogDbContext.BlogFavorites
            .Include(x=>x.Blog)
            .Where(f => f.UserId == userId)
            .Select(f => new BlogDTO
            {
                BlogId = f.Blog.BlogId,
                BlogTitle = f.Blog.BlogTitle,
                BlogContent = f.Blog.BlogContent,
                CreatedAt = f.Blog.CreatedAt,
                UpdatedAt = f.Blog.UpdatedAt,
                CreatedBy = f.Blog.CreatedByUser != null ? new UserDto(f.Blog.CreatedByUser) : null,
                UpdatedBy = f.Blog.UpdatedByUser != null ? new UserDto(f.Blog.UpdatedByUser) : null,
                ApprovedBy = f.Blog.ApprovedByUser != null ? new UserDto(f.Blog.ApprovedByUser) : null,
                StartDate = f.Blog.StartDate,
                EndDate = f.Blog.EndDate,
                ApproveStatus = f.Blog.ApproveStatus,
                ActiveStatus = f.Blog.ActiveStatus,
                Author = f.Blog.Author != null ? new AuthorDto(f.Blog.Author) : null,
                Categories = f.Blog.Categories.Select(c=>new CategoryDto{CategotyId= c.CategoryId,CategoryName = c.CategoryName}).ToList(),
                BlogDocuments = f.Blog.Documents.Select(d=>new BlogDocumentDto{BlogDocumentId = d.BlogDocumentId,DocumentName = d.DocumentName,DocumentType = d.DocumentType}).ToList(),
                isFavorited = true
            })
            .ToListAsync(cancellationToken);
        return Result<IEnumerable<BlogDTO>>.Success(blogs);
    }
}