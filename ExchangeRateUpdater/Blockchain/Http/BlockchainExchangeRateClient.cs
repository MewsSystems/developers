namespace Blockchain.Http {
	using System;
	using System.Net.Http;
	using System.Threading.Tasks;
	using Blockchain.Http.Configuration;
	using ExchangeRateUpdater.Diagnostics;

	public class BlockchainExchangeRateClient : IBlockchainExchangeRateClient {
		private static readonly string requestUriString = "?cors=true";
		private HttpClient _httpClient;

		public BlockchainExchangeRateClient(HttpClient httpClient, IBlockchainHttpClientConfiguration httpConfig) {			
			_httpClient = Ensure.IsNotNull(httpConfig, nameof(httpConfig))
				.Configure(Ensure.IsNotNull(httpClient, nameof(httpClient)));
		}
		public async Task<BlockchainExchangeRateDictionary> GetAsync() {
			var response = await _httpClient.GetAsync(requestUriString);

			var result = await response.EnsureSuccessStatusCode().Content.ReadAsAsync<BlockchainExchangeRateDictionary>();

			return result;
		}

		#region IDisposable implementation
		private bool isDisposed = false;

		void Dispose(bool disposing) {
			if (!isDisposed) {
				if (disposing) {
					_httpClient.Dispose();
					_httpClient = null;
				}

				isDisposed = true;
			}
		}

		public void Dispose() {
			Dispose(true);
		}
		#endregion
	}
}
