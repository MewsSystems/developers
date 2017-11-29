namespace OpenExchangeRates.Http {
	using System.Net.Http;
	using ExchangeRateUpdater.Financial.Http;
	using OpenExchangeRates.Http.Configuration;

	public class OpenExchangeRateClientOptions : HttpExchangeRateClientOptions, IOpenExchangeRateClientOptions {
		public OpenExchangeRateClientOptions(HttpClient httpClient, IOpenExchangeRateHttpClientConfiguration httpConfig)
			: base(httpClient, httpConfig) {
		}
	}
}
