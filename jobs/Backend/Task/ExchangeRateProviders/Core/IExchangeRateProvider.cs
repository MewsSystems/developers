using ExchangeRateProviders.Core.Model;

namespace ExchangeRateProviders.Core;

public interface IExchangeRateProvider
{
    string ExchangeRateProviderCurrencyCode { get; }
	Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, CancellationToken cancellationToken);
}
