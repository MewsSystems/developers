using ExchangeRateUpdater.WebApi.Models;

namespace ExchangeRateUpdater.WebApi.Services.Interfaces
{
    public interface IExchangeRatesParser
    {
        IEnumerable<ExchangeRate> ParseExchangeRates(string formattedExchangeRates);
    }
}
