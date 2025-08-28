using System;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;

namespace BlogApi.Application.Services;

public class BlogService : IBlogService
{
    private readonly IBlogRepository  _blogRepository;

    public BlogService(IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }

    public async Task<Blog> CreateBlogAsync(Blog blog, int userId)
    {
        blog.UserId = userId;
        return await _blogRepository.AddAsync(blog);
    }

    public async Task<IEnumerable<Blog>> GetAllBlogsAsync()
    {
        return await _blogRepository.GetAllAsync();
    }

    public async Task<Blog?> GetBlogByIdAsync(int blogId)
    {
        Blog blog = await _blogRepository.GetByIdAsync(blogId) ?? throw new Exception("Blog not found");
        return blog;
    }
}
