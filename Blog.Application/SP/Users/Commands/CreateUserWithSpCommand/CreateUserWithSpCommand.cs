using BlogApi.Application.DTOs;
using MediatR;

namespace BlogApi.Application.SP.Users.Commands;

public class CreateUserWithSpCommand:IRequest<UserDTO>
{
    public string Name { get; }
    public string Email { get; }
    public string Password { get; set; }
    
    public CreateUserWithSpCommand(string name, string email, string password)
    {
        Name = name;
        Email = email;
        Password = password;
    }
    
}