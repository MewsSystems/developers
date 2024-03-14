using ExchangeRateService.Domain;

namespace ExchangeRateService.Services;

internal interface IExchangeRateProvider
{
    ValueTask<ExchangeRate[]> GetExchangeRatesAsync(IEnumerable<Currency>? currencies);
}