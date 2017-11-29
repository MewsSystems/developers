namespace OpenExchangeRates {
	using ExchangeRateUpdater.Diagnostics;
	using ExchangeRateUpdater.Financial;
	using OpenExchangeRates.Http;

	public class OpenExchangeRateProviderOptions : ExchangeRateProviderOptions, IOpenExchangeRateProviderOptions {
		public OpenExchangeRateProviderOptions(AppId appId, IOpenExchangeRateClient client, ICurrencyValidator validator)
			: base(client, validator) {
			AppId = Ensure.IsNotNull(appId, nameof(appId));
		}

		public AppId AppId { get; }
	}
}
