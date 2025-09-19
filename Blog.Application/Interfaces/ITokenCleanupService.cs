namespace BlogApi.Application.Interfaces;

public interface ITokenCleanupService
{
    Task RemoveExpiredTokensAsync();
}
