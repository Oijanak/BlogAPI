using BlogApi.Application.DTOs;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Application.Interfaces;
using BlogApi.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace BlogApi.Application.Features.Blogs.Queries.GetBlogListQuery;

public class GetBlogListQueryHandler:IRequestHandler<GetBlogListQuery, ApiResponse<IEnumerable<BlogDTO>>>
{
    private readonly IBlogDbContext _blogDbContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDistributedCache _distributedCache;

    public GetBlogListQueryHandler(IBlogDbContext blogDbContext, ICurrentUserService currentUserService,IDistributedCache distributedCache)
    {
        _blogDbContext = blogDbContext;
        _currentUserService = currentUserService;
        _distributedCache = distributedCache;
    }
    
    public async Task<ApiResponse<IEnumerable<BlogDTO>>> Handle(GetBlogListQuery request, CancellationToken cancellationToken)
    {
        var query = _blogDbContext.Blogs.AsNoTracking().AsQueryable();

        var userId = _currentUserService.UserId;
        
        if (request.StartDate.HasValue)
        {
            query = query.Where(b => b.StartDate.Date == request.StartDate.Value.Date);
        }
        if (request.EndDate.HasValue)
        {
            query = query.Where(b => b.EndDate.Date == request.EndDate.Value.Date);
        }

        if (!string.IsNullOrWhiteSpace(request.CreatedBy))
        {
            query = query.Where(b => b.CreatedBy == request.CreatedBy);
        }

        if (!string.IsNullOrWhiteSpace(request.ApprovedBy))
        {
            query = query.Where(b => b.ApprovedBy == request.ApprovedBy);
        }

        if (request.ApproveStatus.HasValue)
        {
            query = query.Where(b => b.ApproveStatus == request.ApproveStatus.Value);
        }

        if (request.ActiveStatus.HasValue)
        {
            query = query.Where(b => b.ActiveStatus == request.ActiveStatus.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.SortBy))
        {
            bool isDesc = request.SortOrder?.ToLower() == "desc";

            query = request.SortBy.ToLower() switch
            {
                "author" => isDesc ? query.OrderByDescending(b => b.Author.AuthorName) : query.OrderBy(b => b.Author.AuthorName),
                "blogtitle"     => isDesc ? query.OrderByDescending(b => b.BlogTitle) : query.OrderBy(b => b.BlogTitle),
                "approvestatus" => isDesc ? query.OrderByDescending(b => b.ApproveStatus) : query.OrderBy(b => b.ApproveStatus),
                "startdate" => isDesc ? query.OrderByDescending(b => b.StartDate) : query.OrderBy(b => b.StartDate),
                "enddate"   => isDesc ? query.OrderByDescending(b => b.EndDate) : query.OrderBy(b => b.EndDate),
        _           => isDesc ? query.OrderByDescending(b => b.CreatedAt) : query.OrderBy(b => b.CreatedAt)
        };
}
        
        
        var cacheKey = $"blogs:Page:{request.Page}:limit:{request.Limit}";
        int skip = (request.Page - 1) * request.Limit;

        var blogDTOs = await _distributedCache.GetOrSetAsync<IEnumerable<BlogDTO>>(cacheKey,
            async () =>
            {
                var blogs = await query
         .Skip(skip)
         .Take(request.Limit)
         .Select(blog => new BlogDTO
         {
             BlogId = blog.BlogId,
             BlogTitle = blog.BlogTitle,
             BlogContent = blog.BlogContent,
             CreatedAt = blog.CreatedAt,
             UpdatedAt = blog.UpdatedAt,
             StartDate = blog.StartDate,
             EndDate = blog.EndDate,
             ActiveStatus = blog.ActiveStatus,
             ApproveStatus = blog.ApproveStatus,
             Author = new AuthorDto
             {
                 AuthorId = blog.Author.AuthorId,
                 AuthorName = blog.Author.AuthorName,
                 AuthorEmail = blog.Author.AuthorEmail,
                 FollowerCount=blog.Author.Followers.Count(),
             },
             CreatedBy = new CreatedByUserDto
             {
                 CreatedById = blog.CreatedByUser.Id,
                 CreatedByName = blog.CreatedByUser.Name,
                 CreatedByEmail = blog.CreatedByUser.Email
             },
             UpdatedBy = blog.UpdatedByUser != null?new UpdatedByUserDto
             {
                 UpdatedById = blog.UpdatedByUser.Id,
                 UpdatedByName = blog.UpdatedByUser.Name,
                 UpdatedByEmail = blog.UpdatedByUser.Email
             }:null,
             ApprovedBy = blog.ApprovedByUser != null ? new ApprovedByUserDto
             {
                 ApprovedById = blog.ApprovedByUser.Id,
                 ApprovedByName = blog.ApprovedByUser.Name,
                 ApprovedByEmail = blog.ApprovedByUser.Email
             } : null,
             BlogDocuments = blog.Documents
                 .Select(d => new BlogDocumentDto
                 {
                     BlogDocumentId = d.BlogDocumentId,
                     DocumentName = d.DocumentName,
                     DocumentType = d.DocumentType
                 }).ToList(),
           
             isFavorited = userId != null ? blog.FavoritedBy.Any(f => f.UserId == userId) : null,
             FavoriteCount=blog.FavoritedBy.Count(),
             CommentCount=blog.Comments.Count(),
         })
         .ToListAsync(cancellationToken);

                return blogs;

            }, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                SlidingExpiration=TimeSpan.FromMinutes(5)
            });
        var totalCount = await query.CountAsync(cancellationToken);

        return new ApiResponse<IEnumerable<BlogDTO>>
        {
            totalSize = totalCount,
            Data = blogDTOs,
            Message = "Blogs fetched successfully"
        };
    }
}