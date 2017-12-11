namespace Blockchain.Http {
	using System.Net.Http;
	using System.Threading.Tasks;
	using ExchangeRateUpdater.Diagnostics;
	using ExchangeRateUpdater.Financial.Http;

	public class BlockchainExchangeRateClient : HttpExchangeRateClient, IBlockchainExchangeRateClient {
		public BlockchainExchangeRateClient(IBlockchainExchangeRateClientOptions options)
			: base(options) { }

		protected override async Task<TResult> ReadContentAsync<TResult>(HttpContent httpContent) {
			var result = await Ensure.IsNotNull(httpContent).ReadAsAsync<TResult>();

			return result;
		}
	}
}
