namespace ApplicationLayer.Common.Exceptions;

/// <summary>
/// Exception thrown when a user does not have permission to perform an action.
/// </summary>
public class ForbiddenException : ApplicationException
{
    public ForbiddenException()
        : base("You do not have permission to perform this action.")
    {
    }

    public ForbiddenException(string message)
        : base(message)
    {
    }
}
