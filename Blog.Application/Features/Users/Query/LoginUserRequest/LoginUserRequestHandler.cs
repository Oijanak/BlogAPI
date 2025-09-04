using System.Net;
using BlogApi.Application.DTOs;
using BlogApi.Application.Exceptions;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using MediatR;
namespace BlogApi.Application.Features.Users.Query.LoginUserRequest;
public class LoginUserRequestHandler : IRequestHandler<LoginUserRequest, LoginResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public LoginUserRequestHandler(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }
    public async Task<LoginResponse> Handle(LoginUserRequest request, CancellationToken cancellationToken)
    {
      User user = await _userRepository.GetUserByEmailAsync(request.Email) ?? throw new ApiException("Invalid Email or Password", HttpStatusCode.Unauthorized);

        bool isPasswordValid = user.VerifyPassword(request.Password);
        if (!isPasswordValid)
        {
            throw new ApiException("Invalid Email or Password", HttpStatusCode.Unauthorized);
        }
        string token = _tokenService.GenerateToken(user.UserId, user.Email);
        return new LoginResponse
        {
            Token = token,
            Expiration = DateTime.UtcNow.AddHours(1)    
        };
     
    }
}