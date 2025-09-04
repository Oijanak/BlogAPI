using System.Net;
using BlogApi.Application.DTOs;
using BlogApi.Application.Exceptions;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using MediatR;

namespace BlogApi.Application.Features.Blogs.Commands.UpdateBlogCommand;

public class UpdateBlogCommandHandler:IRequestHandler<UpdateBlogCommand,BlogDTO>
{
    private readonly IBlogRepository _blogRepository;

    public UpdateBlogCommandHandler(IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }
    public async Task<BlogDTO> Handle(UpdateBlogCommand request, CancellationToken cancellationToken)
    {
        
        Blog existingBlog = await _blogRepository.GetByIdAsync(request.BlogId) ?? throw new ApiException("Blog not found", HttpStatusCode.NotFound);
        if (existingBlog.UserId != request.UserId)
        {
            throw new ApiException("Unauthorized to update this blog", HttpStatusCode.Unauthorized);
        }
        existingBlog.BlogTitle = request.BlogTitle ?? existingBlog.BlogTitle;
        existingBlog.BlogContent = request.BlogContent ?? existingBlog.BlogContent;
         await _blogRepository.Update(existingBlog);
         return new BlogDTO()
         {
             BlogId = existingBlog.BlogId,
             BlogTitle = existingBlog.BlogTitle,
             BlogContent = existingBlog.BlogContent,
             CreatedAt = existingBlog.CreatedAt,
             UpdatedAt = existingBlog.UpdatedAt,
         };
    }
}