using ApplicationLayer.Common.Interfaces;

namespace Integration.Infrastructure;

/// <summary>
/// Fake implementation of IBackgroundJobScheduler for integration tests.
/// Does not actually schedule jobs, just validates parameters.
/// </summary>
public class FakeBackgroundJobScheduler : IBackgroundJobScheduler
{
    public Task RescheduleProviderJobAsync(string providerCode, string updateTime, string timeZone)
    {
        if (string.IsNullOrWhiteSpace(providerCode))
        {
            throw new ArgumentException("Provider code cannot be null or empty.", nameof(providerCode));
        }

        if (string.IsNullOrWhiteSpace(updateTime))
        {
            throw new ArgumentException("Update time cannot be null or empty.", nameof(updateTime));
        }

        if (string.IsNullOrWhiteSpace(timeZone))
        {
            throw new ArgumentException("Time zone cannot be null or empty.", nameof(timeZone));
        }

        // Simulate success without actually scheduling anything
        return Task.CompletedTask;
    }
}
