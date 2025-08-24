using ExchangeRateProviders.Core.Model;

namespace ExchangeRateProviders.Core
{
	public interface IExchangeRateDataProvider
	{
		Task<IEnumerable<ExchangeRate>> GetDailyRatesAsync(CancellationToken cancellationToken);
	}
}
