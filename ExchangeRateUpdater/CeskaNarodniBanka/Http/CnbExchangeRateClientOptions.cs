namespace CeskaNarodniBanka.Http {
	using System.Net.Http;
	using CeskaNarodniBanka.Http.Configuration;
	using ExchangeRateUpdater.Financial.Http;

	public sealed class CnbExchangeRateClientOptions : HttpExchangeRateClientOptions, ICnbExchangeRateClientOptions {
		public CnbExchangeRateClientOptions(HttpClient httpClient, ICnbHttpClientConfiguration httpConfig)
			: base(httpClient, httpConfig) {
		}
	}
}
