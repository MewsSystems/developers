using ApplicationLayer.Common.Interfaces;

namespace Integration.Infrastructure;

/// <summary>
/// Fake implementation of IBackgroundJobService for integration tests.
/// Returns a fake job ID without actually enqueueing jobs.
/// </summary>
public class FakeBackgroundJobService : IBackgroundJobService
{
    public string EnqueueFetchRatesJob(string providerCode)
    {
        if (string.IsNullOrWhiteSpace(providerCode))
        {
            throw new ArgumentException("Provider code cannot be null or empty.", nameof(providerCode));
        }

        // Return a fake job ID for testing purposes
        return $"fake-job-{Guid.NewGuid():N}";
    }
}
