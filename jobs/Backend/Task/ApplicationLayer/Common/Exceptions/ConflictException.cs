namespace ApplicationLayer.Common.Exceptions;

/// <summary>
/// Exception thrown when an operation conflicts with the current state.
/// For example, when trying to create an entity that already exists.
/// </summary>
public class ConflictException : ApplicationException
{
    public ConflictException(string message)
        : base(message)
    {
    }

    public ConflictException(string entityName, string conflictReason)
        : base($"Conflict with '{entityName}': {conflictReason}")
    {
    }
}
