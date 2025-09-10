using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.SP.Authors.Commands.CreateAuthorWithSpCommand;

public class CreateAuthorWithSpCommandHandler:IRequestHandler<CreateAuthorWithSpCommand,AuthorDTO>
{
    private readonly BlogDbContext _blogDbContext;

    public CreateAuthorWithSpCommandHandler(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<AuthorDTO> Handle(CreateAuthorWithSpCommand request, CancellationToken cancellationToken)
    {
        var authors = await _blogDbContext.Authors
            .FromSqlInterpolated($"EXEC spCreateAuthor {request.AuthorEmail}, {request.AuthorName}, {request.Age}")
            .AsNoTracking()
            .ToListAsync(); 

        var author = authors.FirstOrDefault();
        return new AuthorDTO(author);
    }
}