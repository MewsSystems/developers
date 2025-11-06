using DomainLayer.Events.Common;

namespace DomainLayer.Events.UserAggregate;

/// <summary>
/// Event raised when a user's password is changed.
/// </summary>
public sealed record UserPasswordChangedEvent(
    int UserId,
    string Email,
    DateTimeOffset ChangedAt) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}
