namespace ApplicationLayer.Common.Interfaces;

/// <summary>
/// Service for scheduling and managing background jobs.
/// </summary>
public interface IBackgroundJobService
{
    /// <summary>
    /// Enqueues a job to fetch exchange rates for a specific provider immediately.
    /// </summary>
    /// <param name="providerCode">The provider code to fetch rates for.</param>
    /// <returns>The job ID if successful.</returns>
    string EnqueueFetchRatesJob(string providerCode);
}
