using Ardalis.GuardClauses;
using BlogApi.Application.Interfaces;
using BlogApi.Application.SP.Users.Commands.UpdateUserWithSpCommand;

namespace BlogApi.Application.Guards;

public static class UpdateUserWithSpGuard
{
    public static void ValidateWithGuard(UpdateUserWithSpCommand request,IBlogDbContext blogDbContext)
    {
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId));
        Guard.Against.CheckUserExists(request.UserId, nameof(request.UserId),blogDbContext);
        Guard.Against.NullOrEmpty(request.User.Name,nameof(request.User.Name));
        Guard.Against.NullOrEmpty(request.User.Email,nameof(request.User.Email));
        Guard.Against.NullOrEmpty(request.User.Password,nameof(request.User.Password));
        Guard.Against.InvalidEmail(request.User.Email,nameof(request.User.Email));
        
    } 
}