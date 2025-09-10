using BlogApi.Application.DTOs;
using BlogApi.Application.SP.Blogs.Commands;
using BlogApi.Application.SP.Blogs.Commands.DeleteBlogWithSpCommand;
using BlogApi.Application.SP.Blogs.Commands.UpdateBlogWithSpCommand;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.API.Controllers;
[Route("api/sp/blogs")]
[ApiController]
public class BlogControllerWithSp:ControllerBase
{
    
    private readonly ISender _sender;

    public BlogControllerWithSp(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBlog(CreateBlogRequest request)
    {
        BlogDTO blog = await _sender.Send(new CreateBlogWithSpCommand(request.AuthorId,request.BlogTitle,request.BlogContent));
        return Ok(new ApiResponse<BlogDTO>
        {
            Message = "Blog created successfully",
            Data = blog
        });
    }

    [HttpPut("{blogId:guid}")]
    public async Task<IActionResult> UpdateBlog(Guid blogId,UpdateBlogRequest request)
    {
        BlogDTO blog = await _sender.Send(new UpdateBlogWithSpCommand(blogId,request.AuthorId,request.BlogTitle,request.BlogContent));
        return Ok(new ApiResponse<BlogDTO>
        {
            Message = "Blog updated successfully",
            Data = blog
        });
    }

    [HttpDelete("{blogId:guid}")]
    public async Task<IActionResult> DeleteBlog(Guid blogId)
    {
        await _sender.Send(new DeleteBlogWithSpCommand(blogId));
        return Ok(new ApiResponse<string>
        {
            Message = "Blog deleted successfully",
        });
    }
    
}