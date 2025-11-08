namespace ApplicationLayer.Common.Interfaces;

/// <summary>
/// Service for scheduling and managing background jobs.
/// Abstraction over Hangfire or other job scheduling systems.
/// </summary>
public interface IBackgroundJobScheduler
{
    /// <summary>
    /// Reschedules a recurring job for a provider with new schedule.
    /// </summary>
    /// <param name="providerCode">The provider code (e.g., "ECB", "CNB", "BNR")</param>
    /// <param name="updateTime">Time in HH:mm format (e.g., "14:30")</param>
    /// <param name="timeZone">Timezone (e.g., "CET", "UTC", "EET")</param>
    Task RescheduleProviderJobAsync(string providerCode, string updateTime, string timeZone);
}
