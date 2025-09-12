using BlogApi.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Application.SP.Users.Commands.UpdateUserWithSpCommand;

public class UpdateUserWithSpCommand:IRequest<ApiResponse<UserDTO>>
{
    [FromRoute]
    public Guid UserId { get; set; }
    [FromBody]
    public UpdateUserRequest User { get; set; }
    
}