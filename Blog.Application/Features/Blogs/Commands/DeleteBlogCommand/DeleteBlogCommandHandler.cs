using System.Net;
using Ardalis.GuardClauses;
using BlogApi.Application.DTOs;
using BlogApi.Application.Exceptions;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using MediatR;

namespace BlogApi.Application.Features.Blogs.Commands.DeleteBlogCommand;

public class DeleteBlogCommandHandler:IRequestHandler<DeleteBlogCommand,ApiResponse<string>>
{
    private readonly IBlogDbContext _blogDbContext;
    private readonly IFileService _fileService;

    public DeleteBlogCommandHandler(IBlogDbContext blogDbContext, IFileService fileService)
    {
      _blogDbContext = blogDbContext;
      _fileService = fileService;

    }
    public async Task<ApiResponse<string>> Handle(DeleteBlogCommand request, CancellationToken cancellationToken)
    {
        Blog existingBlog = await _blogDbContext.Blogs.FindAsync(request.BlogId);
        Guard.Against.Null(existingBlog,nameof(existingBlog),"Blog cannot be null");
        _blogDbContext.Blogs.Remove(existingBlog);
        await _blogDbContext.SaveChangesAsync(cancellationToken);
        await _fileService.DeleteFilesAsync(request.BlogId);
        return new ApiResponse<string>
        {
            Message = "Blog deleted successfully",
        };
    }
}