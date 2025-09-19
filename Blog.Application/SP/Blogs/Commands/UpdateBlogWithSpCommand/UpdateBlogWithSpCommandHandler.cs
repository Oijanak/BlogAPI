using System.Security.Claims;
using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BlogApi.Application.SP.Blogs.Commands.UpdateBlogWithSpCommand;

public class UpdateBlogWithSpCommandHandler:IRequestHandler<UpdateBlogWithSpCommand,ApiResponse<BlogDTO>>
{
    private readonly IBlogDbContext _blogDbContext;
    private readonly ICurrentUserService _currentUserService;
    public UpdateBlogWithSpCommandHandler(IBlogDbContext blogDbContext,ICurrentUserService currentUserService)
    {
        _blogDbContext = blogDbContext;
        _currentUserService = currentUserService;
    }
    public async Task<ApiResponse<BlogDTO>> Handle(UpdateBlogWithSpCommand request, CancellationToken cancellationToken)
    {
            var currentUserId = _currentUserService.UserId;
            var blogs = await _blogDbContext.Blogs
                .FromSqlInterpolated($"EXEC spUpdateBlog {request.BlogId}, {request.Blog.BlogTitle}, {request.Blog.BlogContent}, {request.Blog.AuthorId},{currentUserId}")
                .AsNoTracking()
                .ToListAsync();
            ArgumentNullException.ThrowIfNull(blogs, nameof(blogs));
            var updatedBlog=blogs.FirstOrDefault();
        
            var blogDto = new BlogDTO
            {
                BlogId = updatedBlog.BlogId,
                BlogTitle = updatedBlog.BlogTitle,
                BlogContent = updatedBlog.BlogContent,
                CreatedAt = updatedBlog.CreatedAt,
                UpdatedAt = updatedBlog.UpdatedAt,
            };
            return new ApiResponse<BlogDTO>
            {
                Data = blogDto,
                Message = "Blog updated successfully",
            };
    }
}