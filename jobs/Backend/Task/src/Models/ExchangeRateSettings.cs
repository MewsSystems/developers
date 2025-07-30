using ExchangeRateUpdater.Interfaces;

namespace ExchangeRateUpdater.Models;

public class ExchangeRateSettings(string url, IExchangeRateParser parser)
{
    public string Url { get; } = url;
    public IExchangeRateParser Parser { get; } = parser;
}