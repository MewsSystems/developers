namespace ExchangeRateUpdater.Lib.Exception;

/// <summary>
/// Represents errors that occur during service execution.
/// This exception is intended to be used for wrapping business logic failures
/// that are not necessarily system errors but indicate an issue within the service.
/// </summary>
public class ServiceException : System.Exception
{
    public ServiceException() : base("Something went wrong.")
    {
    }

    public ServiceException(string message) : base(message)
    {
    }

    public ServiceException(string message, System.Exception innerException)
        : base(message, innerException)
    {
    }
}