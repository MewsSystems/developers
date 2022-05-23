namespace Framework.Exceptions;

/// <summary>
/// Exception used for empty result sets
/// </summary>
public class EmptyResultSetException : Exception
{
    public EmptyResultSetException(string message) : base(message)
    {
        // used for logging
    }
}
