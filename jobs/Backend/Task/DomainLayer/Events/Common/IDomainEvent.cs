namespace DomainLayer.Events.Common;

/// <summary>
/// Marker interface for domain events.
/// Domain events represent something important that happened in the domain.
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// Unique identifier for this event instance.
    /// </summary>
    Guid EventId { get; }

    /// <summary>
    /// The date and time when the event occurred.
    /// </summary>
    DateTimeOffset OccurredOn { get; }
}
