namespace ApplicationLayer.Common.Exceptions;

/// <summary>
/// Base exception for all application layer exceptions.
/// </summary>
public abstract class ApplicationException : Exception
{
    protected ApplicationException(string message)
        : base(message)
    {
    }

    protected ApplicationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
