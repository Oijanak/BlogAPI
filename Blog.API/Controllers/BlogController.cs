using BlogApi.Application.Interfaces;
using BlogApi.Application.DTOs;
using BlogApi.Application.Exceptions;
using BlogApi.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using BlogApi.Application.Features.Blogs.Commands.CreateBlogCommand;
using BlogApi.Application.Features.Blogs.Commands.DeleteBlogCommand;
using BlogApi.Application.Features.Blogs.Commands.UpdateBlogCommand;
using BlogApi.Application.Features.Blogs.Queries.GetBlogListQuery;
using BlogApi.Application.Features.Blogs.Queries.GetBlogQuery;
using MediatR;

namespace BlogApi.API.Controllers;

[Route("api/blogs")]
[ApiController]
public class BlogController : ControllerBase
{
    private readonly ISender  _sender;
    public BlogController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateBlog([FromBody] CreateBlogCommand blog)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId") ?? throw new ApiException("User not authorized", HttpStatusCode.Unauthorized);
        int userId = int.Parse(userIdClaim.Value);
        blog.UserId = userId;
        BlogDTO createdBlog = await _sender.Send(blog);

        return Created("",new ApiResponse<BlogDTO>
        {
            Message = "Blog created successfully",
            Data = createdBlog
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBlogs()
    {
        IEnumerable<BlogDTO> blogDTOs  = await _sender.Send(new GetBlogListQuery());
        return Ok(new ApiResponse<IEnumerable<BlogDTO>>
        {
            Message = "Blogs fetched successfully",
            Data = blogDTOs
        });
    }

    [HttpGet("{blogId}")]
    public async Task<IActionResult> GetBlogById(int blogId)
    {
        BlogDTO blog = await _sender.Send(new GetBlogQuery(blogId));
        return Ok(new ApiResponse<BlogDTO>
        {
            Message = "Blog fetched successfully",
            Data = blog
        });
    }

    [HttpPatch("{blogId}")]
    [Authorize]
    public async Task<IActionResult> UpdateBlog(int blogId, [FromBody] UpdateBlogCommand updateBlogCommand)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId") ?? throw new ApiException("User not authorized", HttpStatusCode.Unauthorized);
        int userId = int.Parse(userIdClaim.Value);
        updateBlogCommand.UserId = userId;
        updateBlogCommand.BlogId = blogId;

        BlogDTO updatedBlog = await _sender.Send(updateBlogCommand);
        return Ok(new ApiResponse<BlogDTO>
        {
            Message = "Blog updated successfully",
            Data = updatedBlog
        });
    }

    [HttpDelete("{blogId}")]
    [Authorize]
    public async Task<IActionResult> DeleteBlog(int blogId)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId") ?? throw new ApiException("User not authorized", HttpStatusCode.Unauthorized);
        int userId = int.Parse(userIdClaim.Value);
        await _sender.Send(new DeleteBlogCommand(blogId,userId));
        return Ok(new ApiResponse<string>
        {
            Message = "Blog deleted successfully",
        });
    }

}