using System.Net;
using Ardalis.GuardClauses;
using BlogApi.Application.DTOs;
using BlogApi.Application.Exceptions;
using BlogApi.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace BlogApi.Application.Features.Users.Commands.RegisterUserCommand;

public class RegisterUserCommandHandler:IRequestHandler<RegisterUserCommand, ApiResponse<string>>
{
    private readonly UserManager<User> _userManager;

    public RegisterUserCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    public async Task<ApiResponse<string>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser=await _userManager.FindByNameAsync(request.Email);
        if (existingUser != null)
        {
            throw new ApiException("User with that email already exists",HttpStatusCode.BadRequest);
        }
        var user = new User
        {
            UserName = request.Email,
            Email = request.Email,
            Name = request.Name
        };
        var result = await _userManager.CreateAsync(user, request.Password);
        return new ApiResponse<string>
        {
            Message = "User Registerd successfully"
        };

    }
}