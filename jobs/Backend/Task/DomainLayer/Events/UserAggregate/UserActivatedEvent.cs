using DomainLayer.Events.Common;

namespace DomainLayer.Events.UserAggregate;

/// <summary>
/// Event raised when a user account is activated.
/// </summary>
public sealed record UserActivatedEvent(
    int UserId,
    string Email,
    DateTimeOffset ActivatedAt) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}
