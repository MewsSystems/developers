using ExchangeRateUpdater.Extensions;

namespace ExchangeRateUpdater.Models;

public class ExchangeRateEndpoint : IExchangeRateEndpoint
{
    public ExchangeRateEndpoint(string name, string url, string parameters)
    {
        Name = name.NotNullOrEmpty();
        Url = url.NotNullOrEmpty();
        Parameters = parameters;
    }

    public string Url { get; }
    public string Parameters { get; }
    public string Name { get; }
}