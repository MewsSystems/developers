using DomainLayer.Enums;
using DomainLayer.Events.Common;

namespace DomainLayer.Events.UserAggregate;

/// <summary>
/// Event raised when a new user is created.
/// </summary>
public sealed record UserCreatedEvent(
    int UserId,
    string Email,
    string FullName,
    UserRole Role,
    DateTimeOffset CreatedAt) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}
