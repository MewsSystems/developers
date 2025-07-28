namespace Mews.ExchangeRateUpdater.Application.Exceptions;

public class RatesAlreadyExistException : Exception
{
    public RatesAlreadyExistException(DateTime date)
        : base($"Exchange rates for {date:yyyy-MM-dd} already exist.") { }
}