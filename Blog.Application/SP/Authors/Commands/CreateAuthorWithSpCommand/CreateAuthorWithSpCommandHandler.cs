using BlogApi.Application.DTOs;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.SP.Authors.Commands.CreateAuthorWithSpCommand;

public class CreateAuthorWithSpCommandHandler:IRequestHandler<CreateAuthorWithSpCommand,ApiResponse<AuthorDto>>
{
    private readonly IBlogDbContext _blogDbContext;

    public CreateAuthorWithSpCommandHandler(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<ApiResponse<AuthorDto>> Handle(CreateAuthorWithSpCommand request, CancellationToken cancellationToken)
    {
        var authors = await _blogDbContext.Authors
            .FromSqlInterpolated($"EXEC spCreateAuthor {request.AuthorEmail}, {request.AuthorName}, {request.Age}")
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