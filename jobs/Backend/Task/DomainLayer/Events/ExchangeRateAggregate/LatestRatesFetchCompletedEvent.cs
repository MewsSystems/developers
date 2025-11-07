using DomainLayer.Events.Common;

namespace DomainLayer.Events.ExchangeRateAggregate;

/// <summary>
/// Event raised when latest exchange rates fetch job completes.
/// </summary>
public sealed record LatestRatesFetchCompletedEvent : IDomainEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTimeOffset OccurredOn { get; init; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// Provider code that was updated.
    /// </summary>
    public string ProviderCode { get; init; } = string.Empty;

    /// <summary>
    /// Number of rates updated.
    /// </summary>
    public int RatesUpdated { get; init; }

    /// <summary>
    /// Whether the fetch was successful.
    /// </summary>
    public bool IsSuccess { get; init; }

    /// <summary>
    /// Optional error message if fetch failed.
    /// </summary>
    public string? ErrorMessage { get; init; }
}
