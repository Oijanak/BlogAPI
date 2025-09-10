using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.SP.Authors.Commands.UpdateAuthorWithSpCommand;

public class UpdateAuthorWithSpCommandHandler:IRequestHandler<UpdateAuthorWithSpCommand, AuthorDTO>
{
    private readonly BlogDbContext _blogDbContext;

    public UpdateAuthorWithSpCommandHandler(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<AuthorDTO> Handle(UpdateAuthorWithSpCommand request, CancellationToken cancellationToken)
    {
        var authors = await _blogDbContext.Authors
            .FromSqlInterpolated($"EXEC spUpdateAuthor {request.AuthorId}, {request.AuthorEmail}, {request.AuthorName}, {request.Age}")
            .AsNoTracking()
            .ToListAsync(); 

        var author = authors.FirstOrDefault();
        return new AuthorDTO(author);
    }
}