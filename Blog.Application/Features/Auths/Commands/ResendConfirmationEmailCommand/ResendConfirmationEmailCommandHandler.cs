using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BlogApi.Application.Features.Auths.Commands.ResendConfirmationEmailCommand
{
    public class ResendConfirmationEmailCommandHandler: IRequestHandler<ResendConfirmationEmailCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly IOptions<DataProtectionTokenProviderOptions> _tokenOptions;

        public ResendConfirmationEmailCommandHandler(
            UserManager<User> userManager,
            IEmailService emailService,
            IOptions<DataProtectionTokenProviderOptions> tokenOptions)
        {
            _userManager = userManager;
            _emailService = emailService;
            _tokenOptions = tokenOptions;
        }

        public async Task<Result<string>> Handle(ResendConfirmationEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Result<string>.Failure("No user found with this email.");

            if (user.EmailConfirmed)
                return Result<string>.Failure("Email is already confirmed.");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = HttpUtility.UrlEncode(token);
            var tokenExpirationMinutes = _tokenOptions.Value.TokenLifespan.TotalMinutes;

            var confirmationLink = $"http://localhost:5173/confirm-email?userId={user.Id}&token={encodedToken}";
            var subject = "Resend: Confirm your email";
            var body = $@"
                <h2>Hello, {user.Name}!</h2>
                <p>You requested to resend your email confirmation link.</p>
                <p>Please confirm your email by clicking below:</p>
                <p>
                    <a href='{confirmationLink}' 
                       style='display:inline-block; padding:10px 20px; color:white; background-color:#007bff; 
                              text-decoration:none; border-radius:5px;'>Confirm Email</a>
                </p>
                <p>This link will expire in {tokenExpirationMinutes} minutes.</p>
            ";

            await _emailService.SendEmailAsync(user.Email, user.Name, subject, body);

            return Result<string>.Success("A new confirmation email has been sent.");
        }
    }
}
