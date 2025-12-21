namespace ExchangeRateProvider.Infrastructure.ExchangeRateProviders;

/// <summary>
/// Defines the contract for cache date/time providers that calculate expiration times for cached exchange rates.
/// This interface abstracts the logic for determining when cached exchange rate data should expire based
/// on specific provider fixing schedules (e.g., CNB publishes at 14:30 Prague time on business days).
/// </summary>
internal interface ICacheDateTimeProvider
{
    /// <summary>
    /// Calculates the UTC expiration time for cached exchange rates based on the provider's fixing schedule.
    /// The implementation should consider the provider's specific publication schedule (business days, time zones, etc.)
    /// to determine the optimal cache expiration time.
    /// </summary>
    /// <param name="buffer">Additional buffer time after the fixing time to account for potential delays or processing time.
    /// If null, the implementation should use a sensible default buffer.</param>
    /// <returns>The UTC DateTime when the cached exchange rates should expire. This ensures that fresh data
    /// will be fetched after the provider's next scheduled update.</returns>
    DateTimeOffset GetNextFixingExpirationUtc(TimeSpan? buffer = null);
}