using Ardalis.GuardClauses;
using BlogApi.Application.DTOs;
using BlogApi.Application.Exceptions;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Enum;
using BlogApi.Domain.Models;
using BlogApi.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Net;

namespace BlogApi.Application.Features.Blogs.Commands.UpdateBlogCommand;

public class UpdateBlogCommandHandler:IRequestHandler<UpdateBlogCommand,ApiResponse<BlogDTO>>
{
    private readonly IBlogDbContext _blogDbContext;
    private readonly IFileService _fileService;
    private readonly IDistributedCache _distributedCache;

    public UpdateBlogCommandHandler(IBlogDbContext blogDbContext, IFileService fileService,IDistributedCache distributedCache)
    {
        _blogDbContext = blogDbContext;
        _fileService = fileService;
        _distributedCache = distributedCache;
    }
    public async Task<ApiResponse<BlogDTO>> Handle(UpdateBlogCommand request, CancellationToken cancellationToken)
    {
        Author author=await _blogDbContext.Authors.FindAsync(request.Blog.AuthorId);
        Guard.Against.Null(author,nameof(author),"Blog cannot be null");
        var categories = await _blogDbContext.Categories
            .Where(c => request.Blog.Categories.Contains(c.CategoryId))
            .ToListAsync(cancellationToken);
        Blog existingBlog = await _blogDbContext.Blogs
            .Include(b => b.CreatedByUser)
            .Include(b => b.UpdatedByUser)
            .Include(b=>b.Categories)
            .FirstOrDefaultAsync(b => b.BlogId == request.BlogId, cancellationToken);
        Guard.Against.Null(existingBlog,nameof(existingBlog));
        existingBlog.BlogTitle = request.Blog.BlogTitle;
        existingBlog.BlogContent = request.Blog.BlogContent;
        existingBlog.StartDate=request.Blog.StartDate;
        existingBlog.EndDate=request.Blog.EndDate;
        existingBlog.Categories.Clear();
        existingBlog.Categories = categories;
        existingBlog.Documents = await _fileService.UpdateFilesAsync(request.BlogId,request.Blog.Documents);
        var currentDate = DateTime.UtcNow.Date;
        if (request.Blog.StartDate.Date <= currentDate && request.Blog.EndDate.Date >= currentDate)
        {
            existingBlog.ActiveStatus = ActiveStatus.Active;
        }
        else
        {
            existingBlog.ActiveStatus = ActiveStatus.Inactive;
        }
        existingBlog.Author = author;
        
         await _blogDbContext.SaveChangesAsync(cancellationToken);
       
        var blogDto= new BlogDTO()
         {
             BlogId = existingBlog.BlogId,
             BlogTitle = existingBlog.BlogTitle,
             BlogContent = existingBlog.BlogContent,
             CreatedAt = existingBlog.CreatedAt,
             UpdatedAt = existingBlog.UpdatedAt,
             CreatedBy = existingBlog.CreatedByUser != null ? new CreatedByUserDto(existingBlog.CreatedByUser) : null,
             UpdatedBy = existingBlog.UpdatedByUser != null ? new UpdatedByUserDto(existingBlog.UpdatedByUser) : null,
             StartDate = existingBlog.StartDate,
             EndDate = existingBlog.EndDate,
             ActiveStatus = existingBlog.ActiveStatus,
             ApproveStatus = existingBlog.ApproveStatus,
             Author = new AuthorDto(existingBlog.Author),
             Categories = categories.Select(c=>new CategoryDto{CategotyId = c.CategoryId,CategoryName = c.CategoryName}).ToList(),
             BlogDocuments = existingBlog.Documents.Select(d=>new BlogDocumentDto{BlogDocumentId = d.BlogDocumentId,DocumentName = d.DocumentName,DocumentType = d.DocumentType}).ToList()
         };
        await _distributedCache.SetAsync<BlogDTO>($"blog:{request.BlogId}",blogDto);

        return new ApiResponse<BlogDTO>
         {
             Data = blogDto,
             Message = "Blog updated successfully"
         };
    }
}