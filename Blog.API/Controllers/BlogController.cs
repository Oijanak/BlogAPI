using BlogApi.Application.Interfaces;
using BlogApi.Application.DTOs;
using BlogApi.Application.Exceptions;
using BlogApi.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using BlogApi.Application.Dapper.Blogs.Commands.CreateBlogWithDapperCommand;
using BlogApi.Application.Dapper.Blogs.Commands.DeleteBlogWithDapperCommand;
using BlogApi.Application.Dapper.Blogs.Commands.UpdateBlogWithDapperCommand;
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
    public async Task<IActionResult> CreateBlog(CreateBlogCommand blogBlogCommand)
    {
        return StatusCode(StatusCodes.Status201Created,await _sender.Send(blogBlogCommand));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBlogs()
    {
        return Ok(await _sender.Send(new GetBlogListQuery()));
       
    }

    [HttpGet("{BlogId:guid}")]
    public async Task<IActionResult> GetBlogById(GetBlogQuery blogQuery)
    {
        return Ok(await _sender.Send(blogQuery));
    }

    [HttpPatch("{BlogId:guid}")]
    [Authorize]
    public async Task<IActionResult> UpdateBlog(UpdateBlogCommand updateBlogCommand)
    {
       return Ok(await _sender.Send(updateBlogCommand));
    }

    [HttpDelete("{BlogId:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteBlog(DeleteBlogCommand deleteBlogCommand)
    {
        return Ok(await _sender.Send(deleteBlogCommand));
    }
    
    [HttpPost("sp")]
    public async Task<IActionResult> CreateBlogWithSp(CreateBlogWithSpCommand createBlogWithSpCommand)
    {
        return StatusCode(StatusCodes.Status201Created,await _sender.Send(createBlogWithSpCommand));
    }

    [HttpPut("sp/{BlogId:guid}")]
    public async Task<IActionResult> UpdateBlogWithSp(UpdateBlogWithSpCommand updateBlogWithSpCommand)
    {
        return Ok(await _sender.Send(updateBlogWithSpCommand));
       
    }

    [HttpDelete("sp/{BlogId:guid}")]
    public async Task<IActionResult> DeleteBlogWithSp(DeleteBlogWithSpCommand deleteBlogWithSpCommand)
    {
        return Ok(await _sender.Send(deleteBlogWithSpCommand));
    }

    [HttpPost("dapper")]
    public async Task<IActionResult> CreateBlogWithDapper(CreateBlogWithDapperCommand createBlogWithDapperCommand)
    {
        return StatusCode(StatusCodes.Status201Created,await _sender.Send(createBlogWithDapperCommand));
    }

    [HttpPut("dapper/{BlogId:guid}")]
    public async Task<IActionResult> UpdateBlogWithDapper(UpdateBlogWithDappersCommand updateBlogWithDappersCommand)
    {
        return Ok(await _sender.Send(updateBlogWithDappersCommand));
    }

    [HttpDelete("dapper/{BlogId:guid}")]
    public async Task<IActionResult> DeleteBlogWithDapper(DeleteBlogWithDapperCommand deleteBlogWithDapperCommand)
    {
        return Ok(await _sender.Send(deleteBlogWithDapperCommand));
    }
    

}