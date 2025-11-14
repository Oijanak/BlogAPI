using Blog.API.Filters;
using BlogApi.Application.Dapper.Authors.CreateAuthorWithDapperCommand;
using BlogApi.Application.Dapper.Authors.UpdateAuthorWithDapperCommand;
using BlogApi.Application.DTOs;
using BlogApi.Application.Features.AuthorFollower.Commands.UnfollowAuthorCommand;
using BlogApi.Application.Features.AuthorFollower.FollowAuthorCommand;
using BlogApi.Application.Features.AuthorFollower.Queries.GetFollowersQuery;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Application.Features.Authors.Commands.DeleteAuthorCommand;
using BlogApi.Application.Features.Authors.Commands.UpdateAuthorCommand;
using BlogApi.Application.Features.Authors.Queries.GetAuthorByIdCommand;
using BlogApi.Application.Features.Authors.Queries.GetAuthorListQuery;
using BlogApi.Application.Features.Authors.Queries.GetAuthorsWithAgeQuery;
using BlogApi.Application.Features.Authors.Queries.GetBlogsByAuthorIdQuery;
using BlogApi.Application.SP.Authors.Commands.CreateAuthorWithSpCommand;
using BlogApi.Application.SP.Authors.Commands.DeleteAuthorWithSpCommand;
using BlogApi.Application.SP.Authors.Commands.UpdateAuthorWithSpCommand;
using BlogApi.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.API.Controllers;
[Route("api/authors")]
[ApiController]
public class AuthorController:ControllerBase
{
    private readonly ISender _sender;
    public AuthorController(ISender sender)
        => _sender = sender;
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateAuthor(CreateAuthorCommand createAuthorCommand)
    {
        return StatusCode(StatusCodes.Status201Created,await _sender.Send(createAuthorCommand));
    }

    [HttpPost("follow/{AuthorId:guid}")]
    [Authorize]
    public async Task<IActionResult> FollowAuthor(FollowAuthorCommand followAuthorCommand)
    {
        var response = await _sender.Send(followAuthorCommand);
        return StatusCode(response.StatusCode, response);
    }
    [HttpPut("{AuthorId:guid}")]
    [Authorize]
    public async Task<IActionResult> UpdateAuthor(UpdateAuthorCommand updateAuthorCommand)
    {
        return Ok(await _sender.Send(updateAuthorCommand));
    }

    [HttpDelete("unfollow/{AuthorId:guid}")]
    [Authorize]
    public async Task<IActionResult> UnfollowAuthor(UnfollowAuthorCommand unfollowAuthorCommand)
    {
        var response = await _sender.Send(unfollowAuthorCommand);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("{AuthorId:guid}")]
    public async Task<IActionResult> DeleteAuthor(DeleteAuthorCommand deleteAuthorCommand)
    {
        return Ok(await _sender.Send(deleteAuthorCommand));
    }

    [HttpGet("{AuthorId:guid}")]
    [Authorize]
    [AllowAnonymous]
    public async Task<IActionResult> GetAuthorById(GetAuthorByIdQuery getAuthorByIdQuery)
    {
        return Ok(await _sender.Send(getAuthorByIdQuery));
    }
    [HttpGet]
    [Authorize]
    [AllowAnonymous]
    public async Task<IActionResult> GetAuthors()
    {
        return Ok(await _sender.Send(new GetAuthorListQuery()));
    }

    [HttpPost("blogs")]
    public async Task<IActionResult> GetBlogsByAuthorId(GetBlogsByAuthorIdQuery getBlogsByAuthorIdQuery)
    {
        return Ok(await _sender.Send(getBlogsByAuthorIdQuery));
    }

    [HttpGet("age")]
    public async Task<IActionResult> GetAuthorsByAgeBetween(GetAuthorsWithAgeQuery getAuthorsWithAgeQuery)
    {
       return Ok(await _sender.Send(getAuthorsWithAgeQuery));
    }

    [HttpGet("followers/{AuthorId:guid}")]
    public async Task<IActionResult> GetAuthorsFollowers(GetFollowersQuery getFollowersQuery)
    {
        var response = await _sender.Send(getFollowersQuery);
        return StatusCode(response.StatusCode, response);
    }
    
    [Authorize]
    [HttpPost("sp")]
    public async Task<IActionResult> CreateAuthorWithSp(CreateAuthorWithSpCommand createAuthorWithSpCommand)
    {
         return  StatusCode(StatusCodes.Status201Created,await _sender.Send(createAuthorWithSpCommand));
    }
    [Authorize]
    [HttpPut("sp/{AuthorId:guid}")]
    public async Task<IActionResult> UpdateAuthorWithSp(UpdateAuthorWithSpCommand updateAuthorWithSpCommand)
    {
        return Ok(await _sender.Send(updateAuthorWithSpCommand));
    }

    [HttpDelete("sp/{AuthorId:guid}")]
    public async Task<IActionResult> DeleteAuthorWithSp(DeleteAuthorWithSpCommand deleteAuthorWithSpCommand)
    {
       return Ok(await _sender.Send(deleteAuthorWithSpCommand));
    }

    [Authorize]
    [HttpPost("dapper")]
    public async Task<IActionResult> CreateAuthorWithDapper(CreateAuthorWithDapperCommand createAuthorWithDapperCommand)
    {
        return StatusCode(StatusCodes.Status201Created, await _sender.Send(createAuthorWithDapperCommand));
    }
    [Authorize]
    [HttpPut("dapper/{AuthorId:guid}")]
    public async Task<IActionResult> UpdateAuthorWithDapper(UpdateAuthorWithDapperCommand updateAuthorWithDapperCommand)
    {
        return Ok(await _sender.Send(updateAuthorWithDapperCommand));
    }
    
    
   
}



