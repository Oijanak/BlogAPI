using BlogApi.Application.Interfaces;
using BlogApi.Domain.DTOs;
using BlogApi.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            return Ok(new ApiResponse<UserDTO>
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            UserDTO? user = await _userService.GetUserByIdAsync(id);
            return Ok(new ApiResponse<UserDTO?>
            {
                Message = "User fetched successfully",
                Data = user
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            ApiResponse<string> response = await _userService.LoginUserAsync(loginRequest);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest updateUser)
        {
            UserDTO updatedUser = await _userService.UpdateUserAsync(id, updateUser);
            return Ok(new ApiResponse<UserDTO>
            {
                Message = "User updated successfully",
                Data = updatedUser
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return Ok(new ApiResponse<string>
            {
                Message = "User deleted successfully",
            });
        }

       
    }
}
