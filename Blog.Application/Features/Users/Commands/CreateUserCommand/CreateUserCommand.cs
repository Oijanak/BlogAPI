using BlogApi.Application.DTOs;
using MediatR;

public class CreateUserCommand : IRequest<UserDTO>
{

    public string Name { get; }
    public string Email { get; }

    public string Password { get; }
    
    public CreateUserCommand(string name, string email, string password)
    {
        Name = name;
        Email = email;
        Password = password;
    }
    
}