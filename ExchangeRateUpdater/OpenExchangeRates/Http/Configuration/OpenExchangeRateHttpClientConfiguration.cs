namespace OpenExchangeRates.Http.Configuration {
	using System;
	using System.Net.Http;
	using ExchangeRateUpdater.Diagnostics;

	public class OpenExchangeRateHttpClientConfiguration : IOpenExchangeRateHttpClientConfiguration {
		public HttpClient Configure(HttpClient target) {
			var httpClient = Ensure.IsNotNull(target);

			httpClient.BaseAddress = new Uri("https://openexchangerates.org/api/");
			httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

			return httpClient;
		}
	}
}
