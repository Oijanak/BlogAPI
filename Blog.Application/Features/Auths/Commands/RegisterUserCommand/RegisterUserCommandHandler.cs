using System.Net;
using Ardalis.GuardClauses;
using BlogApi.Application.DTOs;
using BlogApi.Application.Exceptions;
using BlogApi.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BlogApi.Application.Features.Auths.Commands.RegisterUserCommand;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ApiResponse<string>>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public RegisterUserCommandHandler(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<ApiResponse<string>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        
        var existingUser = await _userManager.FindByNameAsync(request.Email);
        if (existingUser != null)
        {
            throw new ApiException("User with email already exists", HttpStatusCode.BadRequest);
        }
        
        string roleName = request.role.ToString();
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if (!roleResult.Succeeded)
            {
                throw new ApiException("Failed to create role", HttpStatusCode.InternalServerError);
            }
        }
        
        var user = new User
        {
            UserName = request.Email,
            Email = request.Email,
            Name = request.Name,
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            throw new ApiException(result.Errors.First().Description, HttpStatusCode.BadRequest);
        }
        var roleAssignResult = await _userManager.AddToRoleAsync(user, roleName);
        if (!roleAssignResult.Succeeded)
        {
            throw new ApiException("Failed to assign role to user", HttpStatusCode.InternalServerError);
        }

        return new ApiResponse<string>
        {
            Message = "User registered successfully with role: " + roleName
        };
    }
}
