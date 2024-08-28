namespace ExchangeRateUpdater.Application.GetExchangeRates;

public class InvalidCurrencyCodeException : ArgumentException
{
    public InvalidCurrencyCodeException(string currencyCode, ArgumentException inner) : base($"Invalid currency code {currencyCode}.", inner) { }
}