using DomainLayer.Events.Common;

namespace DomainLayer.Events.ProviderAggregate;

/// <summary>
/// Event raised when a provider is activated and made available for fetching rates.
/// </summary>
public sealed record ProviderActivatedEvent(
    int ProviderId,
    string ProviderCode,
    DateTimeOffset ActivatedAt) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}
