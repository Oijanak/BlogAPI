using System.Net;
using BlogApi.Application.DTOs;
using BlogApi.Application.Exceptions;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using BlogApi.Infrastructure.Data;
using MediatR;

namespace BlogApi.Application.Features.Blogs.Commands.UpdateBlogCommand;

public class UpdateBlogCommandHandler:IRequestHandler<UpdateBlogCommand,BlogDTO>
{
    private readonly BlogDbContext _blogDbContext;

    public UpdateBlogCommandHandler(BlogDbContext _blogDbContext)
    {
        _blogDbContext = _blogDbContext;
    }
    public async Task<BlogDTO> Handle(UpdateBlogCommand request, CancellationToken cancellationToken)
    {
        Author author=await _blogDbContext.Authors.FindAsync(request.AuthorId)??throw new ApiException($"Author not found with id {request.AuthorId}", HttpStatusCode.NotFound);
        Blog existingBlog = await _blogDbContext.Blogs.FindAsync(request.BlogId) ?? throw new ApiException($"Blog not found with id {request.BlogId}", HttpStatusCode.NotFound);
        existingBlog.BlogTitle = request.BlogTitle;
        existingBlog.BlogContent = request.BlogContent;
        existingBlog.AuthorId = request.AuthorId;
         _blogDbContext.Blogs.Update(existingBlog);
         await _blogDbContext.SaveChangesAsync(cancellationToken);
         return new BlogDTO()
         {
             BlogId = existingBlog.BlogId,
             BlogTitle = existingBlog.BlogTitle,
             BlogContent = existingBlog.BlogContent,
             CreatedAt = existingBlog.CreatedAt,
             UpdatedAt = existingBlog.UpdatedAt,
             Author = existingBlog.Author,
         };
    }
}