using System;
using BlogApi.Domain.Interfaces;
using BlogApi.Domain.DTOs;
using BlogApi.Domain.Exceptions;
using BlogApi.Domain.Models;
using System.Net;

namespace BlogApi.Application.Services;

public class BlogService : IBlogService
{
    private readonly IBlogRepository _blogRepository;

    private readonly IUserRepository _userRepository;

    public BlogService(IBlogRepository blogRepository, IUserRepository userRepository)
    {
        _blogRepository = blogRepository;
        _userRepository = userRepository;
    }

    public async Task<Blog> CreateBlogAsync(CreateBlogRequest blogRequest, int userId)
    {
        Blog blog = new Blog
        {
            BlogTitle = blogRequest.BlogTitle,
            BlogContent = blogRequest.BlogContent,
            UserId = userId
        };
        return await _blogRepository.AddAsync(blog);
    }

    public async Task DeleteBlogAsync(int blogId, int userId)
    {
        Blog existingBlog = await _blogRepository.GetByIdAsync(blogId) ?? throw new ApiException("Blog not found", HttpStatusCode.NotFound);
        if (existingBlog.UserId != userId)
        {
            throw new ApiException("Unauthorized to delete this blog", HttpStatusCode.Unauthorized);
        }   
        await _blogRepository.Delete(existingBlog);
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

    public async Task<Blog> UpdateBlogAsync(int blogId, UpdateBlogRequest blogRequest, int userId)
    {
        Blog existingBlog = await _blogRepository.GetByIdAsync(blogId) ?? throw new ApiException("Blog not found", HttpStatusCode.NotFound);
        if (existingBlog.UserId != userId)
        {
            throw new ApiException("Unauthorized to update this blog", HttpStatusCode.Unauthorized);
        }
        existingBlog.BlogTitle = blogRequest.BlogTitle ?? existingBlog.BlogTitle;
        existingBlog.BlogContent = blogRequest.BlogContent ?? existingBlog.BlogContent;
        return await _blogRepository.Update(existingBlog);
    }
    


}
