using BlogApi.Application.DTOs;
using BlogApi.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BlogApi.Application.Features.Auths.Commands.ResetPasswordCommand;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result<string>>
{
    private readonly UserManager<User> _userManager;

    public ResetPasswordCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    public async Task<Result<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
            return Result<string>.Failure("Invalid user.");

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

        if (!result.Succeeded)
            return Result<string>.Failure(result.Errors.First().Description);

        return Result<string>.Success("Password has been reset successfully.");
    }
}