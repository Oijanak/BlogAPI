using System.Net;
using BlogApi.Application.DTOs;
using BlogApi.Application.Exceptions;
using BlogApi.Application.Features.Users.Commands.DeleteUserCommand;
using BlogApi.Application.Features.Users.Query.GetUserListQuery;
using BlogApi.Application.Features.Users.Query.GetUserRequest;
using BlogApi.Application.Features.Users.Query.LoginUserRequest;
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
            UserDTO createdUser = await _sender.Send(new CreateUserCommand(user.Name,user.Email,user.Password));
            return Created("",new ApiResponse<UserDTO>
            {
                Message = "User Registered Successfully",
                Data = createdUser
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _sender.Send(new GetUserListQuery());
            return Ok(new ApiResponse<IEnumerable<UserDTO>>
            {
                Message = "Users fetched successfully",
                Data = users
            });
        }

        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetById(Guid userId)
        {
            UserDTO? user = await _sender.Send(new GetUserQuery(userId));
            return Ok(new ApiResponse<UserDTO?>
            {
                Message = "User fetched successfully",
                Data = user
            });
        }
        

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            LoginResponse response = await _sender.Send(new LoginUserRequest(loginRequest.Email,loginRequest.Password));
            return Ok(response);
        }

        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest updateUser)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId") ?? throw new ApiException("User not authorized", HttpStatusCode.Unauthorized);
            Guid userId = Guid.Parse(userIdClaim.Value);
            
            UserDTO updatedUser = await _sender.Send(new UpdateUserCommand(userId,updateUser.Name,updateUser.Email,updateUser.Password));
            return Ok(new ApiResponse<UserDTO>
            {
                Message = "User updated successfully",
                Data = updatedUser
            });
        }

        [HttpDelete("{userId:guid}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            await _sender.Send(new DeleteUserCommand(userId));
            return Ok(new ApiResponse<string>
            {
                Message = "User deleted successfully",
            });
        }

       
    }
}
