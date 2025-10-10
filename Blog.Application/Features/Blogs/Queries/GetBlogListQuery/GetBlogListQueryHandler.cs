using BlogApi.Application.DTOs;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.Features.Blogs.Queries.GetBlogListQuery;

public class GetBlogListQueryHandler:IRequestHandler<GetBlogListQuery, ApiResponse<IEnumerable<BlogDTO>>>
{
    private readonly IBlogDbContext _blogDbContext;

    public GetBlogListQueryHandler(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    
    public async Task<ApiResponse<IEnumerable<BlogDTO>>> Handle(GetBlogListQuery request, CancellationToken cancellationToken)
    {
        var query = _blogDbContext.Blogs
            .Include(blog => blog.Author)
            .Include(blog => blog.CreatedByUser)
            .Include(blog=>blog.Documents)
            .Include(blog => blog.UpdatedByUser)
            .Include(blog=>blog.Categories)
            .Include(blog => blog.ApprovedByUser)
            .AsSplitQuery();;
        
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
        
        var totalCount = await query.CountAsync(cancellationToken);

        int skip = (request.Page - 1) * request.Limit;
        var blogsEntities = await query
            .Skip(skip)
            .Take(request.Limit)
            .ToListAsync(cancellationToken);

        var blogDTOs = blogsEntities.Select(blog => new BlogDTO
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
            ActiveStatus = blog.ActiveStatus,
            ApproveStatus = blog.ApproveStatus,
            Author = blog.Author != null ? new AuthorDto(blog.Author) : null,
            Categories = blog.Categories.Select(c=>new CategoryDto{CategotyId= c.CategoryId,CategoryName = c.CategoryName}).ToList(),
            BlogDocuments = blog.Documents.Select(d=>new BlogDocumentDto{BlogDocumentId = d.BlogDocumentId,DocumentName = d.DocumentName,DocumentType = d.DocumentType}).ToList()
            
        }).ToList();
        return new ApiResponse<IEnumerable<BlogDTO>>
        {
            totalSize = totalCount,
            Data = blogDTOs,
            Message = "Blogs fetched successfully"
        };
    }
}