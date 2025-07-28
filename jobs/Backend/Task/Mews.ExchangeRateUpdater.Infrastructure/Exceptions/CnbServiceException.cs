namespace Mews.ExchangeRateUpdater.Infrastructure.Exceptions;

public class CnbServiceException : Exception
{
    public CnbServiceException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}