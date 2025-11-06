using BlogApi.Application.DTOs;
using BlogApi.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BlogApi.Application.Features.Auths.Commands.ConfirmEmailCommand;

public class ConfirmEmailCommandHandler:IRequestHandler<ConfirmEmailCommand, Result<string>>
{
    private readonly UserManager<User> _userManager;

    public ConfirmEmailCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<Result<string>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user= await _userManager.FindByIdAsync(request.UserId);
        if(user.EmailConfirmed)
            return Result<string>.Success("Email Already  Confirmed");
        var result = await _userManager.ConfirmEmailAsync(user, request.Token);
        if (!result.Succeeded)
        return Result<string>.Failure(result.Errors.First().Description);
        return Result<string>.Success("Email confirmation successfully");
    }
}