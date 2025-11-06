using DomainLayer.Events.Common;

namespace DomainLayer.Events.ProviderAggregate;

/// <summary>
/// Event raised when a new exchange rate provider is created.
/// </summary>
public sealed record ProviderCreatedEvent(
    int ProviderId,
    string ProviderCode,
    string ProviderName,
    int BaseCurrencyId,
    DateTimeOffset CreatedAt) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}
