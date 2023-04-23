using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.ExchangeRateSource;

public interface IExchangeRateSource
{
    Currency CurrencyCode { get; }
    string BaseRestUrl { get; }
    string EndPoint { get; set; }
}