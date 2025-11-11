using DomainLayer.Events.Common;

namespace DomainLayer.Events.ProviderAggregate;

/// <summary>
/// Event raised when a provider is quarantined due to exceeding failure thresholds.
/// This is a critical event that may require immediate attention.
/// </summary>
public sealed record ProviderQuarantinedEvent(
    int ProviderId,
    string ProviderCode,
    int ConsecutiveFailures,
    DateTimeOffset QuarantinedAt) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}
