using BlogApi.Application.Interfaces;
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
        public async Task<IActionResult> RegisterUser([FromBody] User user)
        {
            User createdUser = await _userService.CreateUserAsync(user);
            return Ok(createdUser);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {   
            var users= await _userService.GetAllUsersAsync();
            return Ok(users);
        }
    }
}
