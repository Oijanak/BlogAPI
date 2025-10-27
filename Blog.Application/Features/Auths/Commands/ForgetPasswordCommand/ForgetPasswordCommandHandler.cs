using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace BlogApi.Application.Features.Auths.Commands.ForgetPasswordCommand;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Result<string>>
{
    private readonly UserManager<User> _userManager;
    private readonly IEmailService _emailService;
    private readonly IOptions<DataProtectionTokenProviderOptions> _tokenOptions;

    public ForgotPasswordCommandHandler(UserManager<User> userManager, IEmailService emailService,IOptions<DataProtectionTokenProviderOptions> tokenOptions)
    {
        _userManager = userManager;
        _emailService = emailService;
        _tokenOptions = tokenOptions;
    }

    public async Task<Result<string>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return Result<string>.Failure("User not found.",404);

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        
        var tokenExpirationMinutes = _tokenOptions.Value.TokenLifespan.TotalMinutes;
        await _emailService.SendEmailAsync(
            user.Email,
            user.Name,
            "Password Reset",
            $"<p>UserId :<pre>{user.Id}</pre></p>" +
            $"<p>Token :<pre>{token}</pre></p>"+
            $"<p>This token will expire in {tokenExpirationMinutes} minutes.</p>"
        );

        return Result<string>.Success("Password reset link sent successfully.");
    }
}