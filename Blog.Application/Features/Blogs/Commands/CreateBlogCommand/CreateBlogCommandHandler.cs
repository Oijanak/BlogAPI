using System.Net;
using Ardalis.GuardClauses;
using BlogApi.Application.DTOs;
using BlogApi.Application.Exceptions;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Enum;
using BlogApi.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        var categories = await _blogDbContext.Categories
            .Where(c => request.Categories.Contains(c.CategoryId))
            .ToListAsync(cancellationToken);
        Blog blog = new Blog
        {
            BlogTitle = request.BlogTitle,
            BlogContent = request.BlogContent,
            Author = author,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Categories = categories,
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
               CreatedBy = await _blogDbContext.Users
                            .Where(u => u.Id == blog.CreatedBy)
                            .Select(u => new UserDto(u))
                            .FirstOrDefaultAsync(),
               StartDate = blog.StartDate,
               EndDate = blog.EndDate,
               ActiveStatus = blog.ActiveStatus,
               ApproveStatus = blog.ApproveStatus,

               Author = new AuthorDto(blog.Author),
               Categories = categories.Select(c=>new CategoryDto{CategotyId = c.CategoryId,CategoryName = c.CategoryName}).ToList(),
           },
           Message = "Blog created successfully"
       };
       


    }
}