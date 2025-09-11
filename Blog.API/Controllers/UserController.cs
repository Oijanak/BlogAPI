using System.Net;
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
            _sender= sender;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterUserRequest user)
        {
            return Created("",await _sender.Send(new CreateUserCommand(user.Name,user.Email,user.Password)));
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
           return Ok(await _sender.Send(new GetUserListQuery()));
            
        }

        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetById(Guid userId)
        {
            return Ok(await _sender.Send(new GetUserQuery(userId)));
        }
        

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            return Ok(await _sender.Send(new LoginUserRequest(loginRequest.Email,loginRequest.Password)));
        }

        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest updateUser)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId") ?? throw new ApiException("User not authorized", HttpStatusCode.Unauthorized);
            Guid userId = Guid.Parse(userIdClaim.Value);
            return Ok(await _sender.Send(new UpdateUserCommand(userId,updateUser.Name,updateUser.Email,updateUser.Password)));
          
        }

        [HttpDelete("{userId:guid}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            return Ok(await _sender.Send(new DeleteUserCommand(userId)));
        }
        
        [HttpPost("sp")]
        public async Task<IActionResult> RegisterUserWithSp(RegisterUserRequest user)
        {
           return Created("",await _sender.Send(new CreateUserWithSpCommand(user.Name,user.Email,user.Password)));
        }
    
        [HttpPut("sp/{userId:guid}")]
        public async Task<IActionResult> updateUser(Guid userId,UpdateUserRequest user)
        {
            return Ok(await _sender.Send(new UpdateUserWithSpCommand(userId,user.Name,user.Email,user.Password)));
        }

        [HttpDelete("sp/{userId:guid}")]
        public async Task<IActionResult> DeleteUserWithSp(Guid userId)
        {
            return Ok(await _sender.Send(new DeleteUserWithSpCommand(userId)));
        }

       
    }
}
