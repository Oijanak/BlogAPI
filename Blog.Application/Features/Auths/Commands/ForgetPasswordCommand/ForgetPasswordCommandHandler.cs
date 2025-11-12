using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Web;

namespace BlogApi.Application.Features.Auths.Commands.ForgetPasswordCommand
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly IOptions<DataProtectionTokenProviderOptions> _tokenOptions;

        private readonly string _frontendBaseUrl = "http://localhost:5173";

        public ForgotPasswordCommandHandler(
            UserManager<User> userManager,
            IEmailService emailService,
            IOptions<DataProtectionTokenProviderOptions> tokenOptions)
        {
            _userManager = userManager;
            _emailService = emailService;
            _tokenOptions = tokenOptions;
        }

        public async Task<Result<string>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Result<string>.Failure("User not found.", 404);

          
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var encodedToken = HttpUtility.UrlEncode(token);

            
            var resetLink = $"{_frontendBaseUrl}/reset-password?userId={user.Id}&token={encodedToken}";

            var tokenExpirationMinutes = _tokenOptions.Value.TokenLifespan.TotalMinutes;

            
            var body = $@"
                <h2>Password Reset Request</h2>
                <p>Hello {user.Name},</p>
                <p>We received a request to reset your password. Click the button below to reset it:</p>
                <p><a href=""{resetLink}"" 
                      style=""display:inline-block;background-color:#007bff;color:white;
                             padding:10px 20px;border-radius:6px;text-decoration:none;"">
                    Reset Password
                </a></p>
                <p>This link will expire in {tokenExpirationMinutes} minutes.</p>
                <p>If you didn’t request this, please ignore this email.</p>
            ";

            await _emailService.SendEmailAsync(
                user.Email,
                user.Name,
                "Password Reset",
                body
            );

            return Result<string>.Success("Password reset link sent successfully.");
        }
    }
}
