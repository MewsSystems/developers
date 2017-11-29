namespace Blockchain.Http {
	using System.Net.Http;
	using Blockchain.Http.Configuration;
	using ExchangeRateUpdater.Financial.Http;

	public class BlockchainExchangeRateClientOptions : HttpExchangeRateClientOptions, IBlockchainExchangeRateClientOptions {
		public BlockchainExchangeRateClientOptions(HttpClient httpClient, IBlockchainHttpClientConfiguration httpConfig)
			: base(httpClient, httpConfig) { }
	}
}
