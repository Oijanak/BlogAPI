using System.Net;
using BlogApi.Application.Exceptions;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using MediatR;

namespace BlogApi.Application.Features.Blogs.Commands.DeleteBlogCommand;

public class DeleteBlogCommandHndler:IRequestHandler<DeleteBlogCommand,Unit>
{
    private readonly IBlogRepository _blogRepository;

    public DeleteBlogCommandHndler(IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }
    public async Task<Unit> Handle(DeleteBlogCommand request, CancellationToken cancellationToken)
    {
        Blog existingBlog = await _blogRepository.GetByIdAsync(request.BlogId) ?? throw new ApiException("Blog not found", HttpStatusCode.NotFound);
        if (existingBlog.UserId != request.UserId)
        {
            throw new ApiException("Unauthorized to delete this blog", HttpStatusCode.Unauthorized);
        }   
        await _blogRepository.Delete(existingBlog);
        return Unit.Value;
    }
}