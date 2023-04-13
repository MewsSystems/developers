using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Interfaces
{
    public interface IExchangeRateParser
    {
        IEnumerable<ExchangeRate> ParseExchangeRates(string cnbHtml, Currency targetCurrency, IEnumerable<Currency> currencies);
    }
}
