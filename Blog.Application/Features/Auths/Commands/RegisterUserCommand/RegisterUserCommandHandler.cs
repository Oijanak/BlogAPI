using System.Web;
using Azure.Core;
using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Enum;
using BlogApi.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace BlogApi.Application.Features.Auths.Commands.RegisterUserCommand;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<string>>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IEmailService _emailService;
    private readonly IOptions<DataProtectionTokenProviderOptions> _tokenOptions;

    public RegisterUserCommandHandler(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,IEmailService emailService,IOptions<DataProtectionTokenProviderOptions> tokenOptions)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _emailService = emailService;
        _tokenOptions = tokenOptions;
    }

    public async Task<Result<string>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
            return Result<string>.Failure("User with this email already exists.");

        string roleName = Role.Maker.ToString();

        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if (!roleResult.Succeeded)
                return Result<string>.Failure("Failed to create role.");
        }
        var user = new User
        {
            UserName = request.Email,
            Email = request.Email,
            Name = request.Name
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            return Result<string>.Failure(result.Errors.First().Description);

        var roleAssignResult = await _userManager.AddToRoleAsync(user, roleName);
        if (!roleAssignResult.Succeeded)
            return Result<string>.Failure("Failed to assign role to user.");

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        
        var encodedToken = HttpUtility.UrlEncode(token);
        var tokenExpirationMinutes = _tokenOptions.Value.TokenLifespan.TotalMinutes;
       
               
               var subject = "Confirm your email";
               var body = $@"
                   <h2>Welcome, {user.Name}!</h2>
                   <p>Thank you for registering.:</p>
                    <p><b>UserId :</b><pre>{user.Id}</pre></p>
                   <p><b>Token :</b><pre>{token}</pre></p>
                    <p>This token will expire in {tokenExpirationMinutes} minutes.</p>";
       
               await _emailService.SendEmailAsync(user.Email,user.Name, subject, body);
        return Result<string>.Success($"User registered successfully with role: {roleName}.Please confirm your email");
    }
}