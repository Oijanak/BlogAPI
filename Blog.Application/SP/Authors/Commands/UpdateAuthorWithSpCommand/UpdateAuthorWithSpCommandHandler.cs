using System.Security.Claims;
using BlogApi.Application.DTOs;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.SP.Authors.Commands.UpdateAuthorWithSpCommand;

public class UpdateAuthorWithSpCommandHandler:IRequestHandler<UpdateAuthorWithSpCommand, ApiResponse<AuthorDto>>
{
    private readonly IBlogDbContext _blogDbContext;
    private readonly string _currentUserId;

    public UpdateAuthorWithSpCommandHandler(IBlogDbContext blogDbContext,IHttpContextAccessor httpContextAccessor)
    {
        _blogDbContext = blogDbContext;
         _currentUserId=httpContextAccessor.HttpContext?.User
                    ?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
    public async Task<ApiResponse<AuthorDto>> Handle(UpdateAuthorWithSpCommand request, CancellationToken cancellationToken)
    {
        var authors = await _blogDbContext.Authors
            .FromSqlInterpolated($"EXEC spUpdateAuthor {request.AuthorId}, {request.Author.AuthorEmail}, {request.Author.AuthorName}, {request.Author.Age},{_currentUserId}")
            .AsNoTracking()
            .ToListAsync(); 

        var author = authors.FirstOrDefault();
        return new ApiResponse<AuthorDto>
        {
            Data = new AuthorDto(author),
            Message = "Author updated successfully",
        };
    }
}