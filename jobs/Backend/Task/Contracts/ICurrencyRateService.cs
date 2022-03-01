using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateUpdater.Domain;

namespace ExchangeRateUpdater.Services;

public interface ICurrencyRateService
{
    public Task<IEnumerable<ExchangeRate>> GetCurrencyRatesAsync(
        string exchangeRateCurrencyCode,
        IEnumerable<Currency> fromCurrencies);
}