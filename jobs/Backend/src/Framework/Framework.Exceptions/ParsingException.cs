namespace Framework.Exceptions;

/// <summary>
/// Exception for parsing errors
/// </summary>
public class ParsingException : Exception
{
    public ParsingException(string message) : base(message)
    {
        // used for logging
    }
}
