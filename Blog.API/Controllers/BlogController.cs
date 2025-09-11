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
using BlogApi.Application.SP.Blogs.Commands;
using BlogApi.Application.SP.Blogs.Commands.DeleteBlogWithSpCommand;
using BlogApi.Application.SP.Blogs.Commands.UpdateBlogWithSpCommand;
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
    public async Task<IActionResult> CreateBlog([FromBody] CreateBlogRequest blog)
    {
        return Created("",await _sender.Send(new CreateBlogCommand(blog.AuthorId,blog.BlogTitle, blog.BlogContent)));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBlogs()
    {
        return Ok(await _sender.Send(new GetBlogListQuery()));
       
    }

    [HttpGet("{blogId:guid}")]
    public async Task<IActionResult> GetBlogById(Guid blogId)
    {
        return Ok(await _sender.Send(new GetBlogQuery(blogId)));
    }

    [HttpPatch("{blogId:guid}")]
    [Authorize]
    public async Task<IActionResult> UpdateBlog(Guid blogId, [FromBody]UpdateBlogRequest updateBlog)
    {
       return Ok(await _sender.Send(new UpdateBlogCommand(blogId,updateBlog.AuthorId,updateBlog.BlogTitle,updateBlog.BlogContent)));
    }

    [HttpDelete("{blogId:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteBlog(Guid blogId)
    {
        return Ok(await _sender.Send(new DeleteBlogCommand(blogId)));
    }
    
    [HttpPost("sp")]
    public async Task<IActionResult> CreateBlogWithSp(CreateBlogRequest request)
    {
        return Created("",await _sender.Send(new CreateBlogWithSpCommand(request.AuthorId,request.BlogTitle,request.BlogContent)));
    }

    [HttpPut("sp/{blogId:guid}")]
    public async Task<IActionResult> UpdateBlogWithSp(Guid blogId,UpdateBlogRequest request)
    {
        return Ok(await _sender.Send(new UpdateBlogWithSpCommand(blogId,request.AuthorId,request.BlogTitle,request.BlogContent)));
       
    }

    [HttpDelete("sp/{blogId:guid}")]
    public async Task<IActionResult> DeleteBlogWithSp(Guid blogId)
    {
        return Ok(await _sender.Send(new DeleteBlogWithSpCommand(blogId)));
    }

}