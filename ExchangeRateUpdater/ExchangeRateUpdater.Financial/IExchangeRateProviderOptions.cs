namespace ExchangeRateUpdater.Financial {
	public interface IExchangeRateProviderOptions {
		IExchangeRateClient Client { get; }
		ICurrencyValidator Validator { get; }
	}
}
