using System.Net;
using Ardalis.GuardClauses;
using BlogApi.Application.DTOs;
using BlogApi.Application.Exceptions;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using MediatR;

namespace BlogApi.Application.Features.Blogs.Commands.DeleteBlogCommand;

public class DeleteBlogCommandHndler:IRequestHandler<DeleteBlogCommand,ApiResponse<string>>
{
    private readonly IBlogDbContext _blogDbContext;

    public DeleteBlogCommandHndler(IBlogDbContext blogDbContext)
    {
      _blogDbContext = blogDbContext;
    }
    public async Task<ApiResponse<string>> Handle(DeleteBlogCommand request, CancellationToken cancellationToken)
    {
        Blog existingBlog = await _blogDbContext.Blogs.FindAsync(request.BlogId);
        Guard.Against.Null(existingBlog,nameof(existingBlog));
        _blogDbContext.Blogs.Remove(existingBlog);
        await _blogDbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse<string>
        {
            Message = "Blog deleted successfully",
        };
    }
}