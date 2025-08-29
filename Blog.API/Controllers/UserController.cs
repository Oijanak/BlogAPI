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

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest user)
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
    }
}
