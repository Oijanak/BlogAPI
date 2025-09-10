using BlogApi.Application.DTOs;
using BlogApi.Domain.Models;
using BlogApi.Infrastructure.Data;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BlogApi.Application.SP.Blogs.Commands.UpdateBlogWithSpCommand;

public class UpdateBlogWithSpCommandHandler:IRequestHandler<UpdateBlogWithSpCommand,BlogDTO>
{
    private readonly BlogDbContext _blogDbContext;

    public UpdateBlogWithSpCommandHandler(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;

    }
    public async Task<BlogDTO> Handle(UpdateBlogWithSpCommand request, CancellationToken cancellationToken)
    {
            var blogs = await _blogDbContext.Blogs
                .FromSqlInterpolated($"EXEC spUpdateBlog {request.BlogId}, {request.BlogTitle}, {request.BlogContent}, {request.AuthorId}")
                .AsNoTracking()
                .ToListAsync();
            
            var updatedBlog=blogs.FirstOrDefault();
        
            return new BlogDTO
            {
                BlogId = updatedBlog.BlogId,
                BlogTitle = updatedBlog.BlogTitle,
                BlogContent = updatedBlog.BlogContent,
                CreatedAt = updatedBlog.CreatedAt,
                UpdatedAt = updatedBlog.UpdatedAt,
            };
       


    }
}