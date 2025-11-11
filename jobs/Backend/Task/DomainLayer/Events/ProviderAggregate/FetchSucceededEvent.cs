using DomainLayer.Events.Common;

namespace DomainLayer.Events.ProviderAggregate;

/// <summary>
/// Event raised when a provider successfully fetches exchange rates.
/// </summary>
public sealed record FetchSucceededEvent(
    int ProviderId,
    string ProviderCode,
    int RatesFetched,
    DateTimeOffset FetchedAt) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}
