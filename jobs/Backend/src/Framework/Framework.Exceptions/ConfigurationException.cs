namespace Framework.Exceptions;

/// <summary>
/// Exception used for configuration errors
/// </summary>
public class ConfigurationException : Exception
{
    public ConfigurationException(string message) : base(message)
    {
        // used for logging
    }
}
