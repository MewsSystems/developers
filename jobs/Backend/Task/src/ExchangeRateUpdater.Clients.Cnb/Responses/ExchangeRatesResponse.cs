using ExchangeRateUpdater.Clients.Cnb.Models;

namespace ExchangeRateUpdater.Clients.Cnb.Responses;

public class ExchangeRatesResponse
{
    public List<ExchangeRate> ExchangeRates { get; set; } = new();
}