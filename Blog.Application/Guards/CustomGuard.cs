using System.Text.RegularExpressions;
using Ardalis.GuardClauses;

namespace BlogApi.Application.Guards;

public static class CustomGuard
{
    public static void InvalidEmail(this IGuardClause guardClause, string email, string parameterName)
    {
        Guard.Against.InvalidFormat(email, parameterName, @"^[^@\s]+@[^@\s]+\.[^@\s]+$","Invalid EMail Format");
    }
}