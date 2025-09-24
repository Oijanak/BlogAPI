using BlogApi.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using BlogApi.Domain.Enum;
namespace BlogApi.Infrastructure.Services;

public class UpdateBlogActiveStatusService : IUpdateBlogActiveStatusService
{
    private readonly IBlogDbContext _blogDbContext;
    public UpdateBlogActiveStatusService(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task UpdateBlogActiveStatusAsync()
    {
        var blogs = await _blogDbContext.Blogs.ToListAsync();
            var now = DateTime.UtcNow;

        foreach (var blog in blogs)
        {
            if (blog.StartDate.Date <= now.Date && blog.EndDate >= now.Date)
                blog.ActiveStatus = ActiveStatus.Active;
            else
                blog.ActiveStatus = ActiveStatus.Inactive;

            _blogDbContext.Blogs.Update(blog);
            }
            await _blogDbContext.SaveChangesAsync();
        }
    }
