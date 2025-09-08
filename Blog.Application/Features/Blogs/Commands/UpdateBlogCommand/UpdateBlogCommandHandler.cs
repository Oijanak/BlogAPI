using System.Net;
using BlogApi.Application.DTOs;
using BlogApi.Application.Exceptions;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using BlogApi.Infrastructure.Data;
using MediatR;

namespace BlogApi.Application.Features.Blogs.Commands.UpdateBlogCommand;

public class UpdateBlogCommandHandler:IRequestHandler<UpdateBlogCommand,BlogDTO>
{
    private readonly BlogDbContext _blogDbContext;

    public UpdateBlogCommandHandler(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<BlogDTO> Handle(UpdateBlogCommand request, CancellationToken cancellationToken)
    {
        Author author=await _blogDbContext.Authors.FindAsync(request.AuthorId);
        Blog existingBlog = await _blogDbContext.Blogs.FindAsync(request.BlogId) ;
        existingBlog.BlogTitle = request.BlogTitle;
        existingBlog.BlogContent = request.BlogContent;
        existingBlog.Author = author;
         _blogDbContext.Blogs.Update(existingBlog);
         await _blogDbContext.SaveChangesAsync(cancellationToken);
         return new BlogDTO()
         {
             BlogId = existingBlog.BlogId,
             BlogTitle = existingBlog.BlogTitle,
             BlogContent = existingBlog.BlogContent,
             CreatedAt = existingBlog.CreatedAt,
             UpdatedAt = existingBlog.UpdatedAt,
             Author = new AuthorDTO(existingBlog.Author),
         };
    }
}