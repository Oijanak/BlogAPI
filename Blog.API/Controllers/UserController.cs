using BlogApi.Application.DTOs;
using BlogApi.Application.Features.Users.Queries;
using MediatR;
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
}