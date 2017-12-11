namespace CeskaNarodniBanka.Http.Configuration {
	using System.Net.Http;
	using ExchangeRateUpdater.Configuration;

	public interface ICnbHttpClientConfiguration : IConfiguration<HttpClient> { }
}
