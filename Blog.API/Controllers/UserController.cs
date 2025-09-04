using System.Net;
using BlogApi.Application.DTOs;
using BlogApi.Application.Exceptions;
using BlogApi.Application.Features.Users.Commands.DeleteUserCommand;
using BlogApi.Application.Features.Users.Query.GetBlogsByUserId;
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
        public async Task<IActionResult> RegisterUser([FromBody] CreateUserCommand user)
        {
            UserDTO createdUser = await _sender.Send(user);
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

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetById(int userId)
        {
            UserDTO? user = await _sender.Send(new GetUserQuery(userId));
            return Ok(new ApiResponse<UserDTO?>
            {
                Message = "User fetched successfully",
                Data = user
            });
        }

         [HttpGet("{userId}/blogs")]
        public async Task<IActionResult> GetUserBlogs(int userId)
         {

             var blogs = await _sender.Send(new GetBlogsByUserIdQuery(userId));
             return Ok(new ApiResponse<IEnumerable<BlogDTO>>
             {
                 Message = "User's blogs fetched successfully",
                 Data = blogs
             });
         }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest loginRequest)
        {
            LoginResponse response = await _sender.Send(loginRequest);
            return Ok(response);
        }

        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand updateUser)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId") ?? throw new ApiException("User not authorized", HttpStatusCode.Unauthorized);
            int userId = int.Parse(userIdClaim.Value);
            updateUser.UserId = userId;
            UserDTO updatedUser = await _sender.Send(updateUser);
            return Ok(new ApiResponse<UserDTO>
            {
                Message = "User updated successfully",
                Data = updatedUser
            });
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            await _sender.Send(new DeleteUserCommand(userId));
            return Ok(new ApiResponse<string>
            {
                Message = "User deleted successfully",
            });
        }

       
    }
}
