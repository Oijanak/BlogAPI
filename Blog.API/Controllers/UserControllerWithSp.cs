using BlogApi.Application.DTOs;
using BlogApi.Application.SP.Users.Commands;
using BlogApi.Application.SP.Users.Commands.DeleteUserWithSpCommand;
using BlogApi.Application.SP.Users.Commands.UpdateUserWithSpCommand;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.API.Controllers;
[ApiController]
[Route("api/sp/users")]
public class UserControllerWithSp: ControllerBase
{
    private readonly ISender _sender;

    public UserControllerWithSp(ISender sender)
    {
        _sender = sender;
    }
    [HttpPost]
    public async Task<IActionResult> RegisterUser(RegisterUserRequest user)
    {
        UserDTO createdUser = await _sender.Send(new CreateUserWithSpCommand(user.Name,user.Email,user.Password));
        return Created("",new ApiResponse<UserDTO>
        {
            Message = "User Registered Successfully",
            Data = createdUser
        });
    }
    
    [HttpPut("{userId:guid}")]
    public async Task<IActionResult> updateUser(Guid userId,UpdateUserRequest user)
    {
        UserDTO updatedUser = await _sender.Send(new UpdateUserWithSpCommand(userId,user.Name,user.Email,user.Password));
        return Created("",new ApiResponse<UserDTO>
        {
            Message = "User Updated Successfully",
            Data = updatedUser
        });
    }

    [HttpDelete("{userId:guid}")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        await _sender.Send(new DeleteUserWithSpCommand(userId));
        return Ok(new ApiResponse<string>
        {
            Message = "User deleted successfully",
        });
    }
}