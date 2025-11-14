using BlogApi.Application.DTOs;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using BlogApi.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace BlogApi.Application.Features.Blogs.Queries.GetBlogQuery;

public class GetBlogQueryHandler:IRequestHandler<GetBlogQuery,ApiResponse<BlogDTO>>
{
    private readonly IBlogDbContext _blogDbContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDistributedCache _distributedCache;
    public GetBlogQueryHandler(IBlogDbContext blogDbContext,ICurrentUserService currentUserService,IDistributedCache distributedCache)
    {
        _blogDbContext = blogDbContext;
        _currentUserService = currentUserService;
        _distributedCache = distributedCache;
    }
    
    public async Task<ApiResponse<BlogDTO>> Handle(GetBlogQuery request, CancellationToken cancellationToken)
    {

        var userId = _currentUserService.UserId;
        var cacheKey = $"blog:{request.BlogId}";

        var blogResult = await _distributedCache.GetOrSetAsync<BlogDTO>(cacheKey, async () =>
        {
            var blog = await _blogDbContext.Blogs
                .Where(b => b.BlogId == request.BlogId)
                .Select(b => new BlogDTO
                {
                    BlogId = b.BlogId,
                    BlogTitle = b.BlogTitle,
                    BlogContent = b.BlogContent,
                    CreatedAt = b.CreatedAt,
                    UpdatedAt = b.UpdatedAt,
                    StartDate = b.StartDate,
                    EndDate = b.EndDate,

                    Author = new AuthorDto
                    {
                        AuthorId = b.Author.AuthorId,
                        AuthorName = b.Author.AuthorName,
                        AuthorEmail = b.Author.AuthorEmail,
                        CreatedBy = b.Author.CreatedBy,
                        isFollowed = userId != null
                            ? b.Author.Followers.Any(f => f.UserId == userId)
                            : null,
                        FollowerCount=b.Author.Followers.Count
                    },

                    Categories = b.Categories
                        .Select(c => new CategoryDto
                        {
                            CategotyId = c.CategoryId,
                            CategoryName = c.CategoryName
                        })
                        .ToList(),

                    BlogDocuments = b.Documents
                        .Select(d => new BlogDocumentDto
                        {
                            BlogDocumentId = d.BlogDocumentId,
                            DocumentName = d.DocumentName,
                            DocumentType = d.DocumentType
                        })
                        .ToList(),

                    isFavorited = userId != null
                        ? b.FavoritedBy.Any(f => f.UserId == userId)
                        : null,
                    FavoriteCount = b.FavoritedBy.Count,
                    CommentCount=b.Comments.Count
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return blog;
        },
        new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
            SlidingExpiration = TimeSpan.FromMinutes(2)
        });

        return new ApiResponse<BlogDTO>
        {
            Data = blogResult,
            Message = "Blog fetched successfully"
        };

    }
}