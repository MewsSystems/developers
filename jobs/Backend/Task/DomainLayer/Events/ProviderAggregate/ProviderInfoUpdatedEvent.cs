using DomainLayer.Events.Common;

namespace DomainLayer.Events.ProviderAggregate;

/// <summary>
/// Event raised when a provider's basic information is updated.
/// </summary>
public sealed record ProviderInfoUpdatedEvent(
    int ProviderId,
    string ProviderCode,
    string NewName,
    string NewUrl,
    DateTimeOffset UpdatedAt) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}
