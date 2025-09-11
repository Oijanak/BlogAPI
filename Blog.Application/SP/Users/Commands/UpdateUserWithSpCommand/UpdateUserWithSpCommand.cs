using BlogApi.Application.DTOs;
using MediatR;

namespace BlogApi.Application.SP.Users.Commands.UpdateUserWithSpCommand;

public class UpdateUserWithSpCommand:IRequest<ApiResponse<UserDTO>>
{
    public Guid UserId { get; set; }
    public string Name { get; }
    public string Email { get; }
    public string Password { get; set; }

    public UpdateUserWithSpCommand(Guid userId, string name, string email, string password)
    {
        UserId = userId;
        Name = name;
        Email = email;
        Password = password;
    }   
}