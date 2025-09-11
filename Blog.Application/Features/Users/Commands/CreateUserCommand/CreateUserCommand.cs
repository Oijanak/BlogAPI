using BlogApi.Application.DTOs;
using MediatR;

public class CreateUserCommand : IRequest<ApiResponse<UserDTO>>
{

    public string Name { get; }
    public string Email { get; }
    public string Password { get; set; }
    
    public CreateUserCommand(string name, string email, string password)
    {
        Name = name;
        Email = email;
        Password = password;
    }
    
}