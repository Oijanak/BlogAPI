using BlogApi.Application.DTOs;
using MediatR;

public class CreateUserCommand : IRequest<ApiResponse<UserDTO>>
{

    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    
    public CreateUserCommand(){}
    
    public CreateUserCommand(string name, string email, string password)
    {
        Name = name;
        Email = email;
        Password = password;
    }
    
}