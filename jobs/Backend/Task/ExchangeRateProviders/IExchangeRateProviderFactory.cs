using ExchangeRateProviders.Core;

namespace ExchangeRateProviders
{
	public interface IExchangeRateProviderFactory
	{
		IExchangeRateProvider GetProvider(string exchangeRateProviderCurrencyCode);
	}
}
