using BlogApi.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.SP.Blogs.Commands.DeleteBlogWithSpCommand;

public class DeleteBlogWithSpCommandHandler:IRequestHandler<DeleteBlogWithSpCommand,Unit>
{
    private readonly BlogDbContext _blogDbContext;

    public DeleteBlogWithSpCommandHandler(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<Unit> Handle(DeleteBlogWithSpCommand request, CancellationToken cancellationToken)
    {
        await _blogDbContext.Database
            .ExecuteSqlInterpolatedAsync($"EXEC spDeleteUser {request.BlogId}");
        return Unit.Value;
    }
}