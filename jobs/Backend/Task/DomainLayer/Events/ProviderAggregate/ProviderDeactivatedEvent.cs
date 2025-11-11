using DomainLayer.Events.Common;

namespace DomainLayer.Events.ProviderAggregate;

/// <summary>
/// Event raised when a provider is deactivated and no longer available for fetching rates.
/// </summary>
public sealed record ProviderDeactivatedEvent(
    int ProviderId,
    string ProviderCode,
    DateTimeOffset DeactivatedAt) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}
