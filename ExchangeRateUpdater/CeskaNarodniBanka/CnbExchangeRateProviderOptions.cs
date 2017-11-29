namespace CeskaNarodniBanka {
	using CeskaNarodniBanka.Http;
	using ExchangeRateUpdater.Financial;

	public class CnbExchangeRateProviderOptions : ExchangeRateProviderOptions, ICnbExchangeRateProviderOptions {
		public CnbExchangeRateProviderOptions(ICnbExchangeRateClient client, ICurrencyValidator validator)
			: base(client, validator) { }
	}
}
