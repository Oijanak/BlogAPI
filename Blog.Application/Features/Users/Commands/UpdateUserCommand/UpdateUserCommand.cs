using BlogApi.Application.DTOs;
using MediatR;

public class UpdateUserCommand : IRequest<UserDTO>
{
    public Guid UserId { get; set; }
    public string? Name { get; }
    public string? Email { get; }
    public string? Password { get; }

    public UpdateUserCommand(Guid userId, string? name, string? email, string? password)
    {
        UserId = userId;
        Name = name;
        Email = email;
        Password = password;
    }
}