using ExchangeRateService.Domain;

namespace ExchangeRateService.Services;

internal interface IExchangeRateProvider
{
    ValueTask<ExchangeRate[]> GetExchangeRatesAsync(IReadOnlyList<Currency> currencies, CancellationToken cancellationToken);
}