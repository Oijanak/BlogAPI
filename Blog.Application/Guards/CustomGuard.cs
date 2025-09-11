using System.Text.RegularExpressions;
using Ardalis.GuardClauses;
using BlogApi.Application.Interfaces;

namespace BlogApi.Application.Guards;

public static class CustomGuard
{
    public static void InvalidEmail(this IGuardClause guardClause, string email, string parameterName)
    {
        Guard.Against.InvalidFormat(email, parameterName, @"^[^@\s]+@[^@\s]+\.[^@\s]+$","Invalid EMail Format");
    }

    public static void CheckUserExists(this IGuardClause guardClause, Guid userId, string parameterName,
        IBlogDbContext blogDbContext)
    {
        var exists = blogDbContext.Users.Any(u => u.UserId == userId);
        Guard.Against.Null(exists, nameof(exists),"User does not exist");
    }
}