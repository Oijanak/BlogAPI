using BlogApi.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.SP.Users.Commands.DeleteUserWithSpCommand;

public class DeleteUserWithSpCommandHandler:IRequestHandler<DeleteUserWithSpCommand,Unit>
{
    private readonly BlogDbContext _blogDbContext;

    public DeleteUserWithSpCommandHandler(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<Unit> Handle(DeleteUserWithSpCommand request, CancellationToken cancellationToken)
    {
        await _blogDbContext.Database
            .ExecuteSqlInterpolatedAsync($"EXEC spDeleteBlog {request.UserId}");
        return Unit.Value;
        
    }
}