namespace ExchangeRateUpdater.Financial.Http {
	using System.Net.Http;
	using ExchangeRateUpdater.Configuration;

	public interface IHttpExchangeRateClientOptions {
		HttpClient HttpClient { get; }
		IConfiguration<HttpClient> HttpConfiguration { get; }
	}
}
