using ExchangeRateProvider.Domain.ValueObjects;

namespace ExchangeRateProvider.Domain.Interfaces;

public interface IExchangeRateService
{
    Task<IList<ExchangeRate>>GetExchangeRatesAsync(Currency baseCurrency, CancellationToken cancellationToken = default);
}