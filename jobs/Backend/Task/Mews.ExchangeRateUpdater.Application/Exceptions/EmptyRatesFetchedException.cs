namespace Mews.ExchangeRateUpdater.Application.Exceptions;

public class EmptyRatesFetchedException : Exception
{
    public EmptyRatesFetchedException()
        : base("Fetched exchange rates are empty.") { }
}