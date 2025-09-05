using System.Net;
using BlogApi.Application.Exceptions;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using BlogApi.Infrastructure.Data;
using MediatR;

namespace BlogApi.Application.Features.Blogs.Commands.DeleteBlogCommand;

public class DeleteBlogCommandHndler:IRequestHandler<DeleteBlogCommand,Unit>
{
    private readonly BlogDbContext _blogDbContext;

    public DeleteBlogCommandHndler(BlogDbContext _blogDbContext)
    {
      _blogDbContext = _blogDbContext;
    }
    public async Task<Unit> Handle(DeleteBlogCommand request, CancellationToken cancellationToken)
    {
        Blog existingBlog = await _blogDbContext.Blogs.FindAsync(request.BlogId)?? throw new ApiException("Blog not found", HttpStatusCode.NotFound);
        _blogDbContext.Blogs.Remove(existingBlog);
        await _blogDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}