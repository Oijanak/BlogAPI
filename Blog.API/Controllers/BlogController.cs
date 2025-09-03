using BlogApi.Application.Interfaces;
using BlogApi.Application.DTOs;
using BlogApi.Application.Exceptions;
using BlogApi.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BlogApi.API.Controllers;

[Route("api/blogs")]
[ApiController]
public class BlogController : ControllerBase
{
    private readonly IBlogService _blogService;
    public BlogController(IBlogService blogService)
    {
        _blogService = blogService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateBlog([FromBody] CreateBlogRequest blog)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId") ?? throw new ApiException("User not authorized", HttpStatusCode.Unauthorized);
        int userId = int.Parse(userIdClaim.Value);
        var createdBlog = await _blogService.CreateBlogAsync(blog, userId);

        return Created("",new ApiResponse<BlogDTO>
        {
            Message = "Blog created successfully",
            Data = new BlogDTO
            {
                BlogId = createdBlog.BlogId,
                BlogTitle = createdBlog.BlogTitle,
                BlogContent = createdBlog.BlogContent,
                CreatedAt = createdBlog.CreatedAt,
                UpdatedAt = createdBlog.UpdatedAt
            }
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBlogs()
    {
        var blogs = await _blogService.GetAllBlogsAsync();
        var blogDTOs = blogs.Select(b => new BlogDTO
        {
            BlogId = b.BlogId,
            BlogTitle = b.BlogTitle,
            BlogContent = b.BlogContent,
            CreatedAt = b.CreatedAt,
            UpdatedAt = b.UpdatedAt
        });

        return Ok(new ApiResponse<IEnumerable<BlogDTO>>
        {
            Message = "Blogs fetched successfully",
            Data = blogDTOs
        });
    }

    [HttpGet("{blogId}")]
    public async Task<IActionResult> GetBlogById(int blogId)
    {
        Blog blog = await _blogService.GetBlogByIdAsync(blogId) ?? throw new ApiException("Blog not found", HttpStatusCode.NotFound);
        var blogDTO = new BlogDTO
        {
            BlogId = blog.BlogId,
            BlogTitle = blog.BlogTitle,
            BlogContent = blog.BlogContent,
            CreatedAt = blog.CreatedAt,
            UpdatedAt = blog.UpdatedAt
        };
        return Ok(new ApiResponse<BlogDTO>
        {
            Message = "Blog fetched successfully",
            Data = blogDTO
        });
    }

    [HttpPatch("{blogId}")]
    [Authorize]
    public async Task<IActionResult> UpdateBlog(int blogId, [FromBody] UpdateBlogRequest updateBlog)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId") ?? throw new ApiException("User not authorized", HttpStatusCode.Unauthorized);
        int userId = int.Parse(userIdClaim.Value);

        var updatedBlog = await _blogService.UpdateBlogAsync(blogId, updateBlog, userId);
        return Ok(new ApiResponse<BlogDTO>
        {
            Message = "Blog updated successfully",
            Data = new BlogDTO
            {
                BlogId = updatedBlog.BlogId,
                BlogTitle = updatedBlog.BlogTitle,
                BlogContent = updatedBlog.BlogContent,
                CreatedAt = updatedBlog.CreatedAt,
                UpdatedAt = updatedBlog.UpdatedAt
            }
        });
    }

    [HttpDelete("{blogId}")]
    [Authorize]
    public async Task<IActionResult> DeleteBlog(int blogId)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId") ?? throw new ApiException("User not authorized", HttpStatusCode.Unauthorized);
        int userId = int.Parse(userIdClaim.Value);
        await _blogService.DeleteBlogAsync(blogId, userId);
        return Ok(new ApiResponse<string>
        {
            Message = "Blog deleted successfully",
        });
    }

}