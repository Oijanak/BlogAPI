using System.Security.Claims;
using BlogApi.Application.DTOs;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.SP.Authors.Commands.CreateAuthorWithSpCommand;

public class CreateAuthorWithSpCommandHandler:IRequestHandler<CreateAuthorWithSpCommand,ApiResponse<AuthorDto>>
{
    private readonly IBlogDbContext _blogDbContext;
    private readonly ICurrentUserService _currentUserService;

    public CreateAuthorWithSpCommandHandler(IBlogDbContext blogDbContext,ICurrentUserService currentUserService)
    {
        _blogDbContext = blogDbContext;
        _currentUserService = currentUserService;
    }
    public async Task<ApiResponse<AuthorDto>> Handle(CreateAuthorWithSpCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;
        var authors = await _blogDbContext.Authors
            .FromSqlInterpolated($"EXEC spCreateAuthor {request.AuthorEmail}, {request.AuthorName}, {request.Age},{currentUserId}")
            .AsNoTracking()
            .ToListAsync(); 

        var author = authors.FirstOrDefault();
        return new ApiResponse<AuthorDto>
        {
            Data = new AuthorDto(author),
            Message = "Author created successfully",
        };
    }
}