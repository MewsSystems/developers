namespace Logging.Exceptions;

public class XmlParsingException : Exception
{
    public XmlParsingException(string message) : base(message)
    {
        // used for logging middleware
    }
}
