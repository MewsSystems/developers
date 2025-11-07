using DomainLayer.Events.Common;

namespace DomainLayer.Events.ExchangeRateAggregate;

/// <summary>
/// Event raised when historical exchange rates fetch job completes.
/// </summary>
public sealed record HistoricalRatesFetchCompletedEvent : IDomainEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTimeOffset OccurredOn { get; init; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// Number of rates fetched.
    /// </summary>
    public int RatesFetched { get; init; }

    /// <summary>
    /// Whether the fetch was successful.
    /// </summary>
    public bool IsSuccess { get; init; }

    /// <summary>
    /// Optional error message if fetch failed.
    /// </summary>
    public string? ErrorMessage { get; init; }
}
