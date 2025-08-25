using ExchangeRateProviders.Core.Model;

namespace ExchangeRateProviders.Core
{
	public interface IExchangeRateDataProvider
	{
		string ExchangeRateProviderTargetCurrencyCode { get; }
		Task<IEnumerable<ExchangeRate>> GetDailyRatesAsync(CancellationToken cancellationToken);
	}
}
