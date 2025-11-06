using DomainLayer.Events.Common;

namespace DomainLayer.Events.ProviderAggregate;

/// <summary>
/// Event raised when a provider configuration setting is added or updated.
/// </summary>
public sealed record ProviderConfigurationSetEvent(
    int ProviderId,
    string ProviderCode,
    string ConfigurationKey,
    string ConfigurationValue,
    DateTimeOffset SetAt) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}
