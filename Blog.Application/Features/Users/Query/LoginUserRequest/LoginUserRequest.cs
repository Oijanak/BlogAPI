using BlogApi.Application.DTOs;
using MediatR;
namespace BlogApi.Application.Features.Users.Query.LoginUserRequest;
public class LoginUserRequest : IRequest<LoginResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }

    
}