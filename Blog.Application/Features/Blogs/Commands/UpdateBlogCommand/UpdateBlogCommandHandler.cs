using System.Net;
using Ardalis.GuardClauses;
using BlogApi.Application.DTOs;
using BlogApi.Application.Exceptions;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Enum;
using BlogApi.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.Features.Blogs.Commands.UpdateBlogCommand;

public class UpdateBlogCommandHandler:IRequestHandler<UpdateBlogCommand,ApiResponse<BlogDTO>>
{
    private readonly IBlogDbContext _blogDbContext;

    public UpdateBlogCommandHandler(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<ApiResponse<BlogDTO>> Handle(UpdateBlogCommand request, CancellationToken cancellationToken)
    {
        Author author=await _blogDbContext.Authors.FindAsync(request.Blog.AuthorId);
        Guard.Against.Null(author,nameof(author),"Blog cannot be null");
        Blog existingBlog = await _blogDbContext.Blogs
            .Include(b => b.CreatedByUser)
            .Include(b => b.UpdatedByUser)
            .FirstOrDefaultAsync(b => b.BlogId == request.BlogId, cancellationToken);
        Guard.Against.Null(existingBlog,nameof(existingBlog));
        existingBlog.BlogTitle = request.Blog.BlogTitle;
        existingBlog.BlogContent = request.Blog.BlogContent;
        existingBlog.StartDate=request.Blog.StartDate;
        existingBlog.EndDate=request.Blog.EndDate;
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
         _blogDbContext.Blogs.Update(existingBlog);
         await _blogDbContext.SaveChangesAsync(cancellationToken);
         var blogDto= new BlogDTO()
         {
             BlogId = existingBlog.BlogId,
             BlogTitle = existingBlog.BlogTitle,
             BlogContent = existingBlog.BlogContent,
             CreatedAt = existingBlog.CreatedAt,
             UpdatedAt = existingBlog.UpdatedAt,
             CreatedBy = existingBlog.CreatedByUser != null ? new UserDto(existingBlog.CreatedByUser) : null,
             UpdatedBy = existingBlog.UpdatedByUser != null ? new UserDto(existingBlog.UpdatedByUser) : null,
             StartDate = existingBlog.StartDate,
             EndDate = existingBlog.EndDate,
             ActiveStatus = existingBlog.ActiveStatus,
             ApproveStatus = existingBlog.ApproveStatus,
             Author = new AuthorDto(existingBlog.Author),
         };
         return new ApiResponse<BlogDTO>
         {
             Data = blogDto,
             Message = "Blog updated successfully"
         };
    }
}