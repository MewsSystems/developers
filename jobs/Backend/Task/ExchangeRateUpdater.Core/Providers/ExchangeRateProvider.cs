using ExchangeRateUpdater.Core.Models;

namespace ExchangeRateUpdater.Core.Providers;

internal class ExchangeRateProvider : IExchangeRateProvider
{
	public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
	{
		return Enumerable.Empty<ExchangeRate>();
	}
}