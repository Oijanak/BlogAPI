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
        var now = DateTime.UtcNow.Date;


        await _blogDbContext.Blogs
            .Where(b => b.StartDate.Date <= now && b.EndDate.Date >= now)
            .ExecuteUpdateAsync(b => b.SetProperty(blog => blog.ActiveStatus, blog => ActiveStatus.Active));

        
        await _blogDbContext.Blogs
            .Where(b => b.StartDate.Date > now || b.EndDate.Date < now)
            .ExecuteUpdateAsync(b => b.SetProperty(blog => blog.ActiveStatus, blog => ActiveStatus.Inactive));
    }
}
