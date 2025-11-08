namespace ExchangeRateProviders.Core
{
	public interface IExchangeRateDataProviderFactory
	{
		IExchangeRateDataProvider GetProvider(string exchangeRateProviderCurrencyCode);
	}
}
