using BlogApi.Domain.Interfaces;
using BlogApi.Domain.DTOs;
using BlogApi.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BlogApi.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRequest user)
        {
            UserDTO createdUser = await _userService.RegisterUserAsync(user);
            return Created("",new ApiResponse<UserDTO>
            {
                Message = "User Registered Successfully",
                Data = createdUser
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(new ApiResponse<IEnumerable<UserDTO>>
            {
                Message = "Users fetched successfully",
                Data = users
            });
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetById(int userId)
        {
            UserDTO? user = await _userService.GetUserByIdAsync(userId);
            return Ok(new ApiResponse<UserDTO?>
            {
                Message = "User fetched successfully",
                Data = user
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            LoginResponse response = await _userService.LoginUserAsync(loginRequest);
            return Ok(response);
        }

        [HttpPatch("{userId}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UpdateUserRequest updateUser)
        {
            UserDTO updatedUser = await _userService.UpdateUserAsync(userId, updateUser);
            return Ok(new ApiResponse<UserDTO>
            {
                Message = "User updated successfully",
                Data = updatedUser
            });
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            await _userService.DeleteUserAsync(userId);
            return Ok(new ApiResponse<string>
            {
                Message = "User deleted successfully",
            });
        }

       
    }
}
