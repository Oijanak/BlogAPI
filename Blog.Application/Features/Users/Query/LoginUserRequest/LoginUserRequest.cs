using BlogApi.Application.DTOs;
using MediatR;
namespace BlogApi.Application.Features.Users.Query.LoginUserRequest;
public class LoginUserRequest : IRequest<LoginResponse>
{
    public string Email { get;}
    public string Password { get;}

    public LoginUserRequest(string email, string password)
    {
        Email = email;
        Password = password;
    }
}