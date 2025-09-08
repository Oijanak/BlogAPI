using System.Net;
using BlogApi.Application.Exceptions;
using BlogApi.Domain.Models;
using BlogApi.Infrastructure.Data;
using MediatR;

namespace BlogApi.Application.Features.Authors.Commands.DeleteAuthorCommand;

public class DeleteAuthorCommandHandler:IRequestHandler<DeleteAuthorCommand,Unit>
{
    private readonly BlogDbContext _blogDbContext;

    public DeleteAuthorCommandHandler(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<Unit> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
    {
        Author author=await _blogDbContext.Authors.FindAsync(request.AuthorId);
        _blogDbContext.Authors.Remove(author);
        await _blogDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}