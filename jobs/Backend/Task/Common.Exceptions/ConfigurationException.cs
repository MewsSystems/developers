namespace Common.Exceptions;

public class ConfigurationException : Exception
{
    #region Constructors

    public ConfigurationException(string? message) : base(message) { }

    #endregion
}