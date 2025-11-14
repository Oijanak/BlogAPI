using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Enum;
using BlogApi.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Features.Blogs.Queries.GetAllPulicBlogsQuery;

public class GetAllPublicBlogsQueryHandler : IRequestHandler<GetAllPublicBlogsQuery, ApiResponse<IEnumerable<BlogPublicDto>>>
{
    private readonly IBlogDbContext _blogDbContext;
    private readonly ICurrentUserService _currentUserService;
  

    public GetAllPublicBlogsQueryHandler(IBlogDbContext blogDbContext, ICurrentUserService currentUserService)
    {
        _blogDbContext = blogDbContext;
        _currentUserService = currentUserService;
       
    }


    public async Task<ApiResponse<IEnumerable<BlogPublicDto>>> Handle(GetAllPublicBlogsQuery request, CancellationToken cancellationToken)
    {
        var query = _blogDbContext.Blogs.AsNoTracking().AsQueryable().Where(b=>b.ApproveStatus==ApproveStatus.Approved);

        var userId = _currentUserService.UserId;

        if (!string.IsNullOrWhiteSpace(request.Author))
        {
            query = query.Where(b => b.Author.AuthorName == request.Author);
        }
        if (request.CategoryNames != null && request.CategoryNames.Any())
        {
            var lowerCaseNames = request.CategoryNames.Select(c => c.ToLower()).ToList();

            query = query.Where(b =>
                b.Categories.Any(c =>
                    lowerCaseNames.Contains(c.CategoryName.ToLower())
                )
            );
        }

        if (!string.IsNullOrWhiteSpace(request.SortBy))
        {
            bool isDesc = request.SortOrder?.ToLower() == "desc";

            query = request.SortBy.ToLower() switch
            {
                "author" => isDesc ? query.OrderByDescending(b => b.Author.AuthorName) : query.OrderBy(b => b.Author.AuthorName),
                "blogtitle" => isDesc ? query.OrderByDescending(b => b.BlogTitle) : query.OrderBy(b => b.BlogTitle),
                _ => isDesc ? query.OrderByDescending(b => b.CreatedAt) : query.OrderBy(b => b.CreatedAt)
            };
        }
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var keyword = request.Search.Trim().ToLower();

            query = query.Where(b =>
                b.BlogTitle.ToLower().Contains(keyword) ||
                b.BlogContent.ToLower().Contains(keyword)
            );
        }

       
        int skip = (request.Page - 1) * request.Limit;

        
            var blogs = await query
         .Skip(skip)
         .Take(request.Limit)
         .Select(blog => new BlogPublicDto
         {
             BlogId = blog.BlogId,
             BlogTitle = blog.BlogTitle,
             BlogContent = blog.BlogContent,
             CreatedAt = blog.CreatedAt,
             UpdatedAt = blog.UpdatedAt,
             Author = new AuthorDto
             {
                 AuthorId = blog.Author.AuthorId,
                 AuthorName = blog.Author.AuthorName,
                 AuthorEmail = blog.Author.AuthorEmail,
                 FollowerCount = blog.Author.Followers.Count(),
             },
             Categories = blog.Categories.Select(c => new CategoryDto
             {
                 CategotyId = c.CategoryId,
                 CategoryName = c.CategoryName,
             }).ToList(),
             BlogDocuments = blog.Documents
                 .Select(d => new BlogDocumentDto
                 {
                     BlogDocumentId = d.BlogDocumentId,
                     DocumentName = d.DocumentName,
                     DocumentType = d.DocumentType
                 }).ToList(),
             isFavorited = userId != null ? blog.FavoritedBy.Any(f => f.UserId == userId) : null,
             FavoriteCount = blog.FavoritedBy.Count(),
             CommentCount = blog.Comments.Count(),
         })
         .ToListAsync(cancellationToken);

                
        var totalCount = await query.CountAsync(cancellationToken);

        return new ApiResponse<IEnumerable<BlogPublicDto>>
        {
            totalSize = totalCount,
            Data = blogs,
            Message = "Blogs fetched successfully"
        };
    }
}
