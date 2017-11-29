namespace ExchangeRateUpdater.Financial {
	using ExchangeRateUpdater.Diagnostics;

	public class ExchangeRateProviderOptions : IExchangeRateProviderOptions {
		public ExchangeRateProviderOptions(IExchangeRateClient client, ICurrencyValidator validator) {
			Client = Ensure.IsNotNull(client);
			Validator = Ensure.IsNotNull(validator);
		}

		public IExchangeRateClient Client { get; }
		public ICurrencyValidator Validator { get; }
	}
}