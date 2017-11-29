namespace ExchangeRateUpdater.Financial.Http {
	using System;
	using System.Net.Http;
	using ExchangeRateUpdater.Configuration;
	using ExchangeRateUpdater.Diagnostics;

	public abstract class HttpExchangeRateClientOptions : IHttpExchangeRateClientOptions {
		public HttpExchangeRateClientOptions(HttpClient httpClient, IConfiguration<HttpClient> httpConfig) {
			HttpClient = Ensure.IsNotNull(httpClient);
			HttpConfiguration = Ensure.IsNotNull(httpConfig);
		}

		public HttpClient HttpClient { get; }

		public IConfiguration<HttpClient> HttpConfiguration { get; }
	}
}
