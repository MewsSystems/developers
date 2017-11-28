namespace Blockchain.Http.Configuration {
	using System.Net.Http;
	using ExchangeRateUpdater.Configuration;

	public interface IBlockchainHttpClientConfiguration : IConfiguration<HttpClient> {
	}
}
