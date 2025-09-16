using BlogApi.Application.DTOs;
using MediatR;

namespace BlogApi.Application.Dapper.Users.Commands;

public class CreateUserWithDapperCommand:IRequest<ApiResponse<UserDTO>>  
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    
}