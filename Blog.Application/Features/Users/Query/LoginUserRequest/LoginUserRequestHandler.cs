using System.Net;
using BlogApi.Application.DTOs;
using BlogApi.Application.Exceptions;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.Features.Users.Query.LoginUserRequest;
public class LoginUserRequestHandler : IRequestHandler<LoginUserRequest, LoginResponse>
{
    private readonly IBlogDbContext _blogDbContext;
    private readonly ITokenService _tokenService;

    public LoginUserRequestHandler(IBlogDbContext blogDbContext, ITokenService tokenService)
    {
        _blogDbContext=blogDbContext;
        _tokenService = tokenService;
    }
    public async Task<LoginResponse> Handle(LoginUserRequest request, CancellationToken cancellationToken)
    {
        User user = await _blogDbContext.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email) ?? throw new ApiException("Invalid Email or Password", HttpStatusCode.Unauthorized);

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