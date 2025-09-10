using BlogApi.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.SP.Authors.Commands.DeleteAuthorWithSpCommand;

public class DeleteAuthorWithSpCommandHandler:IRequestHandler<DeleteAuthorWithSpCommand, Unit>
{
    private readonly BlogDbContext _blogDbContext;

    public DeleteAuthorWithSpCommandHandler(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<Unit> Handle(DeleteAuthorWithSpCommand request, CancellationToken cancellationToken)
    {
        await _blogDbContext.Database
            .ExecuteSqlInterpolatedAsync($"EXEC spDeleteAuthor {request.AuthorId}");
        return Unit.Value;
    }
}