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

public class UpdateBlogWithSpCommandHandler:IRequestHandler<UpdateBlogWithSpCommand,ApiResponse<string>>
{
    private readonly IBlogDbContext _blogDbContext;
    private readonly ICurrentUserService _currentUserService;
    public UpdateBlogWithSpCommandHandler(IBlogDbContext blogDbContext,ICurrentUserService currentUserService)
    {
        _blogDbContext = blogDbContext;
        _currentUserService = currentUserService;
    }
    public async Task<ApiResponse<string>> Handle(UpdateBlogWithSpCommand request, CancellationToken cancellationToken)
    {
            var currentUserId = _currentUserService.UserId;
             await _blogDbContext.Database
                .ExecuteSqlInterpolatedAsync($"EXEC spUpdateBlog {request.BlogId}, {request.Blog.BlogTitle}, {request.Blog.BlogContent},{request.Blog.StartDate},{request.Blog.EndDate}, {request.Blog.AuthorId},{currentUserId}")
                ;
        
            return new ApiResponse<string>
            {
                Message = "Blog updated successfully",
            };
    }
}