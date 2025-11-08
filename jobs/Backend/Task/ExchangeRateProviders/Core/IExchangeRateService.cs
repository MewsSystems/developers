using ExchangeRateProviders.Core.Model;

namespace ExchangeRateProviders.Core;

public interface IExchangeRateService
{
	Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(string TargetCurrency, IEnumerable<Currency> currencies, CancellationToken cancellationToken);
}
