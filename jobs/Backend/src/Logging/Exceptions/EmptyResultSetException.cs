namespace Logging.Exceptions;

public class EmptyResultSetException : Exception
{
    public EmptyResultSetException(string message) : base(message)
    {
        // used for logging middleware
    }
}
