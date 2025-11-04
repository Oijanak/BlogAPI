using BlogApi.Application.Features.Roles.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.API.Controllers
{
    [ApiController]
    [Route("api/roles")]
    public class RoleController:ControllerBase
    {
        private readonly ISender _sender;
        public RoleController(ISender sender)
        {
            _sender = sender;
        }
        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetAllRoles()
        {
            var result=await _sender.Send(new GetAllRolesQueries());
            return StatusCode(result.StatusCode, result);
        }

    }
}
