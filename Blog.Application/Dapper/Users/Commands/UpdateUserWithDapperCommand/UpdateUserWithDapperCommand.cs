using BlogApi.Application.DTOs;
using BlogApi.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Application.Dapper.Users.Commands.UpdateUserWithDapperCommand;

public class UpdateUserWithDapperCommand:IRequest<ApiResponse<UserDTO>>
{
    [FromRoute]
    public Guid UserId { get; set; }
    [FromBody]
    public UpdateUserRequest User{get; set; }
    
}