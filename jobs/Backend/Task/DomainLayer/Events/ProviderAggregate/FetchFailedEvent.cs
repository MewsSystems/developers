using DomainLayer.Events.Common;

namespace DomainLayer.Events.ProviderAggregate;

/// <summary>
/// Event raised when a provider fails to fetch exchange rates.
/// </summary>
public sealed record FetchFailedEvent(
    int ProviderId,
    string ProviderCode,
    int ConsecutiveFailures,
    string? ErrorMessage,
    DateTimeOffset FailedAt) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}
