using BlogApi.Application.DTOs;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.SP.Authors.Commands.UpdateAuthorWithSpCommand;

public class UpdateAuthorWithSpCommandHandler:IRequestHandler<UpdateAuthorWithSpCommand, ApiResponse<AuthorDto>>
{
    private readonly IBlogDbContext _blogDbContext;

    public UpdateAuthorWithSpCommandHandler(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<ApiResponse<AuthorDto>> Handle(UpdateAuthorWithSpCommand request, CancellationToken cancellationToken)
    {
        var authors = await _blogDbContext.Authors
            .FromSqlInterpolated($"EXEC spUpdateAuthor {request.AuthorId}, {request.AuthorEmail}, {request.AuthorName}, {request.Age}")
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