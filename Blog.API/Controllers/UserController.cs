using System.Net;
using Blog.API.Filters;
using BlogApi.Application.Dapper.Users.Commands;
using BlogApi.Application.Dapper.Users.Commands.DeleteUserWithDapperCommand;
using BlogApi.Application.Dapper.Users.Commands.UpdateUserWithDapperCommand;
using BlogApi.Application.DTOs;
using BlogApi.Application.Exceptions;
using BlogApi.Application.Features.Users.Commands.DeleteUserCommand;
using BlogApi.Application.Features.Users.Query.GetUserListQuery;
using BlogApi.Application.Features.Users.Query.GetUserRequest;
using BlogApi.Application.Features.Users.Query.LoginUserRequest;
using BlogApi.Application.SP.Users.Commands;
using BlogApi.Application.SP.Users.Commands.DeleteUserWithSpCommand;
using BlogApi.Application.SP.Users.Commands.UpdateUserWithSpCommand;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BlogApi.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ISender _sender;

        public UserController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(CreateUserCommand createUserCommand)
        {
            return StatusCode(StatusCodes.Status201Created, await _sender.Send(createUserCommand));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _sender.Send(new GetUserListQuery()));

        }

        [HttpGet("{UserId:guid}")]
        public async Task<IActionResult> GetById(GetUserQuery getUserQuery)
        {
            return Ok(await _sender.Send(getUserQuery));
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserRequest loginRequest)
        {
            return Ok(await _sender.Send(loginRequest));
        }

        [HttpPatch("{UserId:guid}")]
        [Authorize]
        [AuthorizeUser]
        public async Task<IActionResult> UpdateUser(UpdateUserCommand updateUserCommand)
        {
            return Ok(await _sender.Send(updateUserCommand));
        }

        [HttpDelete("{UserId:guid}")]
        public async Task<IActionResult> DeleteUser(DeleteUserCommand deleteUserCommand)
        {
            return Ok(await _sender.Send(deleteUserCommand));
        }

        [HttpPost("sp")]
        public async Task<IActionResult> RegisterUserWithSp(CreateUserWithSpCommand createUserWithSpCommand)
        {
            return StatusCode(StatusCodes.Status201Created, await _sender.Send(createUserWithSpCommand));
        }

        [HttpPut("sp/{UserId:guid}")]
        public async Task<IActionResult> UpdateUserSp(UpdateUserWithSpCommand updateUserWithSpCommand)
        {
            return Ok(await _sender.Send(updateUserWithSpCommand));
        }

        [HttpDelete("sp/{UserId:guid}")]
        public async Task<IActionResult> DeleteUserWithSp(DeleteUserWithSpCommand deleteUserWithSpCommand)
        {
            return Ok(await _sender.Send(deleteUserWithSpCommand));
        }

        //Using Dapper

        [HttpPost("dapper")]
        public async Task<IActionResult> CreateUserUsingDapper(CreateUserWithDapperCommand command)
        {
            return StatusCode(StatusCodes.Status201Created, await _sender.Send(command));
        }

        [HttpPut("dapper/{UserId:guid}")]
        public async Task<IActionResult> UpdateUserUsingDapper(UpdateUserWithDapperCommand updateUserWithDapperCommand)
        {
            return Ok(await _sender.Send(updateUserWithDapperCommand));
        }

        [HttpDelete("dapper/{UserId:guid}")]
        public async Task<IActionResult> DeleteUserUsingDapper(DeleteUserWithDapperCommand deleteUserWithSpCommand)
        {
            return Ok(await _sender.Send(deleteUserWithSpCommand));
        }
    }
}