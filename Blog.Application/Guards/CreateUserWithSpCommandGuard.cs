using System.Text.RegularExpressions;
using Ardalis.GuardClauses;
using BlogApi.Application.SP.Users.Commands;

namespace BlogApi.Application.Guards;

public static class CreateUserWithSpCommandGuard
{

    public static void ValidateWithGuard(CreateUserWithSpCommand request)
    {
        Guard.Against.NullOrEmpty(request.Name,nameof(request.Name));
        Guard.Against.NullOrEmpty(request.Email,nameof(request.Email));
        Guard.Against.NullOrEmpty(request.Password,nameof(request.Password));
        Guard.Against.InvalidEmail(request.Email,nameof(request.Email));
    }
}