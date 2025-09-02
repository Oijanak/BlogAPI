using BlogApi.Domain.DTOs;
using BlogApi.Domain.Models;

namespace BlogApi.Domain.Interfaces;

public interface IBlogService
{
    Task<Blog> CreateBlogAsync(CreateBlogRequest blog, int userId);
    Task<IEnumerable<Blog>> GetAllBlogsAsync();
    Task<Blog?> GetBlogByIdAsync(int blogId);
    Task<Blog> UpdateBlogAsync(int blogId, UpdateBlogRequest blogRequest, int userId);

    Task DeleteBlogAsync(int blogId, int userId);
}
