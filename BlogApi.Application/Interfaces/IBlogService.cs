using BlogApi.Domain.Models;

namespace BlogApi.Application.Interfaces;

public interface IBlogService
{
Task<Blog> CreateBlogAsync(Blog blog, int userId);
Task<IEnumerable<Blog>> GetAllBlogsAsync();
Task<Blog?> GetBlogByIdAsync(int blogId);
}
