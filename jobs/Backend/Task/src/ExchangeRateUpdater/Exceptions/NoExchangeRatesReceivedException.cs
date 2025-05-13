namespace ExchangeRateUpdater.Exceptions;

public class NoExchangeRatesReceivedException(string url) : Exception("External service returned null or an empty list of exchange rates.")
{
    public string Url { get; } = url;
}
