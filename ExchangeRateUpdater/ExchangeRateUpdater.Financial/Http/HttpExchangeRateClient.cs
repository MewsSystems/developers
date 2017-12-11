namespace ExchangeRateUpdater.Financial.Http {
	using System.Net.Http;
	using System.Threading.Tasks;
	using ExchangeRateUpdater.Diagnostics;

	public abstract class HttpExchangeRateClient : IExchangeRateClient {
		private HttpClient _httpClient;

		public HttpExchangeRateClient(IHttpExchangeRateClientOptions options) {
			Throw.IfNull(options);

			_httpClient = Ensure.IsNotNull(options.HttpConfiguration, nameof(options.HttpConfiguration))
				.Configure(Ensure.IsNotNull(options.HttpClient, nameof(options.HttpClient)));
		}

		public async Task<TResult> GetAsync<TResult>(string location) {
			Throw.IfNullOrWhiteSpace(location);

			var response = await _httpClient.GetAsync(location);

			var result = await ReadContentAsync<TResult>(response.EnsureSuccessStatusCode().Content);

			return result;
		}

		protected abstract Task<TResult> ReadContentAsync<TResult>(HttpContent httpContent);

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
