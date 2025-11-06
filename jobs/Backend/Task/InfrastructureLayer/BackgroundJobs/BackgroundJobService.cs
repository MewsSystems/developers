using ApplicationLayer.Common.Interfaces;
using Hangfire;
using InfrastructureLayer.BackgroundJobs.Jobs;
using Microsoft.Extensions.Logging;

namespace InfrastructureLayer.BackgroundJobs;

/// <summary>
/// Service for managing background jobs using Hangfire.
/// </summary>
public class BackgroundJobService : IBackgroundJobService
{
    private readonly ILogger<BackgroundJobService> _logger;

    public BackgroundJobService(ILogger<BackgroundJobService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Enqueues a job to fetch exchange rates for a specific provider immediately.
    /// </summary>
    public string EnqueueFetchRatesJob(string providerCode)
    {
        if (string.IsNullOrWhiteSpace(providerCode))
        {
            throw new ArgumentException("Provider code cannot be null or empty.", nameof(providerCode));
        }

        var jobId = BackgroundJob.Enqueue<FetchLatestRatesJob>(
            job => job.ExecuteAsync(providerCode, CancellationToken.None));

        _logger.LogInformation(
            "Enqueued fetch rates job for provider {ProviderCode}. Job ID: {JobId}",
            providerCode,
            jobId);

        return jobId;
    }
}
