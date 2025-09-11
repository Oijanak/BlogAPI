using System.Net;
using Ardalis.GuardClauses;
using BlogApi.Application.DTOs;
using BlogApi.Application.Exceptions;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using MediatR;

namespace BlogApi.Application.Features.Blogs.Commands.CreateBlogCommand;

public class CreateBlogCommandHandler:IRequestHandler<CreateBlogCommand,ApiResponse<BlogDTO>>
{
    private readonly IBlogDbContext _blogDbContext;

    public CreateBlogCommandHandler(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    
    public async Task<ApiResponse<BlogDTO>> Handle(CreateBlogCommand request, CancellationToken cancellationToken)
    {
        Author author=await _blogDbContext.Authors.FindAsync(request.AuthorId);
        Guard.Against.Null(author,nameof(author));
        Blog blog = new Blog
        {
            BlogTitle = request.BlogTitle,
            BlogContent = request.BlogContent,
            Author = author
        };
        
       await _blogDbContext.Blogs.AddAsync(blog, cancellationToken);
       await _blogDbContext.SaveChangesAsync(cancellationToken);
       return new ApiResponse<BlogDTO>
       {
           Data = new BlogDTO()
           {
               BlogId = blog.BlogId,
               BlogTitle = blog.BlogTitle,
               BlogContent = blog.BlogContent,
               CreatedAt = blog.CreatedAt,
               UpdatedAt = blog.UpdatedAt,
               Author = new AuthorDto(blog.Author)
           },
           Message = "Blog created successfully"
       };





    }
}