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
        Guard.Against.NullOrEmpty(request.Name,nameof(request.Name));
        Guard.Against.NullOrEmpty(request.Email,nameof(request.Email));
        Guard.Against.NullOrEmpty(request.Password,nameof(request.Password));
        Guard.Against.InvalidEmail(request.Email,nameof(request.Email));
        
    } 
}