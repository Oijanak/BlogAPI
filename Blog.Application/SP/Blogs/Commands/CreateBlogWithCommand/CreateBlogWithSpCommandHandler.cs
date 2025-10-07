using System.Security.Claims;
using Ardalis.GuardClauses;
using BlogApi.Application.DTOs;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.SP.Blogs.Commands;

public class CreateBlogWithSpCommandHandler:IRequestHandler<CreateBlogWithSpCommand,ApiResponse<string>>
{
    private readonly IBlogDbContext _blogDbContext;
    private readonly ICurrentUserService _currentUserService;
    public CreateBlogWithSpCommandHandler(IBlogDbContext blogDbContext,ICurrentUserService  currentUserService)
    {
        _blogDbContext = blogDbContext;
        _currentUserService = currentUserService;
    }
    public async Task<ApiResponse<string>> Handle(CreateBlogWithSpCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;
        Guard.Against.NullOrEmpty(currentUserId, nameof(currentUserId),"current user ID is null or empty");
        await _blogDbContext.Database
            .ExecuteSqlInterpolatedAsync($"EXEC spCreateBlog {request.AuthorId}, {request.BlogTitle}, {request.BlogContent},{request.StartDate},{request.EndDate},{currentUserId}")
            ;
        return new ApiResponse<string>
        {
            Message = "Blog created successfully"
        };

    }
}
