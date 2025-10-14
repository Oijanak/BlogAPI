using BlogApi.Application.DTOs;
using BlogApi.Application.Features.AuthorFollower.Queries.GetFollowingAuthorsQuery;
using BlogApi.Application.Features.BlogFavorite.Queries.GetFavoriteBlogsQuery;
using BlogApi.Application.Features.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.API.Controllers;
[ApiController]
[Route("api/users")]
public class UserController:ControllerBase
{
    private readonly ISender _sender;

    public UserController(ISender sender)
    {
        _sender = sender;
    }
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        return Ok(await _sender.Send(new GetAllUsersQuery()));
    }

    [HttpGet("following")]
    [Authorize]
    public async Task<IActionResult> GetFollowingUsers()
    {
        var response = await _sender.Send(new GetFollowingAuthorsQuery());
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("favorites")]
    [Authorize]
    public async Task<IActionResult> GetFavorites()
    {
        var response = await _sender.Send(new GetFavoriteBlogsQuery());
        return StatusCode(response.StatusCode, response);
    }
}