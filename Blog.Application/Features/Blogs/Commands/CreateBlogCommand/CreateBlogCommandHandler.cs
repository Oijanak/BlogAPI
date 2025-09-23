using System.Net;
using Ardalis.GuardClauses;
using BlogApi.Application.DTOs;
using BlogApi.Application.Exceptions;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Enum;
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
        Guard.Against.Null(author,nameof(author),"Author cannot be null");
        Blog blog = new Blog
        {
            BlogTitle = request.BlogTitle,
            BlogContent = request.BlogContent,
            Author = author,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
        };
        
        var currentDate = DateTime.UtcNow.Date; 

        if (request.StartDate.Date <= currentDate && request.EndDate.Date >= currentDate)
        {
            blog.ActiveStatus = ActiveStatus.Active;
        }
        else
        {
            blog.ActiveStatus = ActiveStatus.Inactive;
        }
        
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
               CreatedBy = blog.CreatedBy,
               StartDate = blog.StartDate,
               EndDate = blog.EndDate,
               ActiveStatus = blog.ActiveStatus,
               ApproveStatus = blog.ApproveStatus,
               ApprovedBy = blog.ApprovedBy,
               Author = new AuthorDto(blog.Author)
           },
           Message = "Blog created successfully"
       };
       


    }
}