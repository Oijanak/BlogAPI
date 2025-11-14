using BlogApi.Application.DTOs;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Application.Interfaces;
using DocumentFormat.OpenXml.Math;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.Features.Authors.Queries.GetBlogsByAuthorIdQuery;

public class GetBlogsAuthorIdQueryHandler:IRequestHandler<GetBlogsByAuthorIdQuery,ApiResponse<IEnumerable<BlogPublicDto>>>
{
    private readonly IBlogDbContext _blogDbContext;
    private readonly ICurrentUserService _currentUserService;

    public GetBlogsAuthorIdQueryHandler(IBlogDbContext blogDbContext, ICurrentUserService currentUserService)
    {
        _blogDbContext = blogDbContext;
        _currentUserService = currentUserService;
    }
    public async Task<ApiResponse<IEnumerable<BlogPublicDto>>> Handle(GetBlogsByAuthorIdQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        var skip = (request.Page - 1) * request.Limit;
        var query = _blogDbContext.Blogs.Where(blog => blog.AuthorId == request.AuthorId).AsNoTracking().AsQueryable();
        var blogDtos= await query
            .Select(blog => new BlogPublicDto
            {
                BlogId = blog.BlogId,
                BlogTitle = blog.BlogTitle,
                BlogContent = blog.BlogContent,
                CreatedAt = blog.CreatedAt,
                UpdatedAt = blog.UpdatedAt,
                BlogDocuments = blog.Documents
                 .Select(d => new BlogDocumentDto
                 {
                     BlogDocumentId = d.BlogDocumentId,
                     DocumentName = d.DocumentName,
                     DocumentType = d.DocumentType
                 }).ToList(),
                Categories=blog.Categories.Select(c=>new CategoryDto
                {
                    CategotyId=c.CategoryId,
                    CategoryName=c.CategoryName
                }).ToList(),
                isFavorited = userId != null ? blog.FavoritedBy.Any(f => f.UserId == userId) : null,
                FavoriteCount = blog.FavoritedBy.Count(),
                CommentCount = blog.Comments.Count(),


            }).Skip(skip).Take(request.Limit).
            ToListAsync();
        int size=await query.CountAsync();
       
        return new ApiResponse<IEnumerable<BlogPublicDto>>
        {
            Data = blogDtos,
            totalSize= size,
            Message = "Authors blogs fetched successfully"
        };

    }
}