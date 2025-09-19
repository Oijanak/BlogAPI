using System.Security.Claims;
using Ardalis.GuardClauses;
using BlogApi.Application.DTOs;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.SP.Blogs.Commands;

public class CreateBlogWithSpCommandHandler:IRequestHandler<CreateBlogWithSpCommand,ApiResponse<BlogDTO>>
{
    private readonly IBlogDbContext _blogDbContext;
    private readonly ICurrentUserService _currentUserService;
    public CreateBlogWithSpCommandHandler(IBlogDbContext blogDbContext,ICurrentUserService  currentUserService)
    {
        _blogDbContext = blogDbContext;
        _currentUserService = currentUserService;
    }
    public async Task<ApiResponse<BlogDTO>> Handle(CreateBlogWithSpCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;
        Guard.Against.NullOrEmpty(currentUserId, nameof(currentUserId),"current user ID is null or empty");
        var blogs = await _blogDbContext.Blogs
            .FromSqlInterpolated($"EXEC spCreateBlogWithAuthor {request.AuthorId}, {request.BlogTitle}, {request.BlogContent},{currentUserId}")
            .AsNoTracking()
            .ToListAsync();
        Guard.Against.Null(blogs,nameof(blogs),"Blogs cannot be null");
        var result=blogs.FirstOrDefault();
        var blogDtos=new BlogDTO
        {
            BlogId = result.BlogId,
            BlogTitle = result.BlogTitle,
            BlogContent = result.BlogContent,
            CreatedAt = result.CreatedAt,
            UpdatedAt = result.UpdatedAt,
        };
        return new ApiResponse<BlogDTO>
        {
            Data = blogDtos,
            Message = "Blog created successfully"
        };

    }
}
