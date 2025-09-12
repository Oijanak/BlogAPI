using BlogApi.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

public class UpdateUserCommand : IRequest<ApiResponse<UserDTO>>
{
    [FromRoute]
    public Guid UserId { get; set; }
   [FromBody]
    public UpdateUserRequest User{get; set; }
    
}