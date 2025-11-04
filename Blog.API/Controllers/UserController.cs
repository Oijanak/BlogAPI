using BlogApi.Application.Features.Users.Commands.ChangeUserPasswordCommand;
using BlogApi.Application.DTOs;
using BlogApi.Application.Features.AuthorFollower.Queries.GetFollowingAuthorsQuery;
using BlogApi.Application.Features.BlogFavorite.Queries.GetFavoriteBlogsQuery;
using BlogApi.Application.Features.Users.Commands.AddUserAsMakerCommand;
using BlogApi.Application.Features.Users.Commands.AddUserRoles;
using BlogApi.Application.Features.Users.Commands.RemoveUserRoleCommand;
using BlogApi.Application.Features.Users.Queries;
using DocumentFormat.OpenXml.Wordprocessing;
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
    
    [HttpPost("add-roles")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddUserRole( AddUserRoleCommand command)
    {
        var result = await _sender.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("remove-roles")]
    [Authorize(Roles ="Admin")]
    public async Task<IActionResult> RemoveUserRoles(RemoveUserRoleCommand removeUserRole)
    {
        var result=await _sender.Send(removeUserRole);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("make-maker")]
    [Authorize(Roles ="Admin")]
    public async Task<IActionResult> MakeUserMaker(AddUserAsMakerCommand addUserAsMakerCommand)
    {
        var result=await _sender.Send(addUserAsMakerCommand);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPatch("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(ChangeUserPasswordCommand changeUserPasswordCommand)
    {
        var result= await _sender.Send(changeUserPasswordCommand);
        return StatusCode(result.StatusCode, result);
    }
}