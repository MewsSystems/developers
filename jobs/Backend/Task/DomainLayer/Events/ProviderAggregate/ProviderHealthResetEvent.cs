using DomainLayer.Events.Common;

namespace DomainLayer.Events.ProviderAggregate;

/// <summary>
/// Event raised when a provider's health status is manually reset after intervention.
/// </summary>
public sealed record ProviderHealthResetEvent(
    int ProviderId,
    string ProviderCode,
    DateTimeOffset ResetAt) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}
