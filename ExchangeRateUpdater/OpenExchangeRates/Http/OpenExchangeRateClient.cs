namespace OpenExchangeRates.Http {
	using System.Collections.Generic;
	using System.Linq;
	using System.Net.Http;
	using System.Text;
	using System.Threading.Tasks;
	using ExchangeRateUpdater.Diagnostics;
	using OpenExchangeRates.Http.Configuration;

	public class OpenExchangeRateClient : IOpenExchangeRateClient {
		private HttpClient _httpClient;

		public OpenExchangeRateClient(IOpenExchangeRateClientOptions options, HttpClient httpClient, IOpenExchangeRateHttpClientConfiguration httpConfig) {
			Options = Ensure.IsNotNull(options);

			_httpClient = Ensure.IsNotNull(httpConfig, nameof(httpConfig))
				.Configure(Ensure.IsNotNull(httpClient, nameof(httpClient)));
		}

		public IOpenExchangeRateClientOptions Options { get; }

		public async Task<OpenExchangeRate> GetAsync(IEnumerable<string> currencies) {
			var sb = currencies.Aggregate(new StringBuilder($"latest.json?app_id={Options.AppId}&symbols="), (feed, item) => feed.AppendFormat("{0},", item));

			sb.Remove(sb.Length - 1, 1);
			string requestUri = sb.ToString();

			var response = await _httpClient.GetAsync(requestUri);

			var result = await response.EnsureSuccessStatusCode().Content.ReadAsAsync<OpenExchangeRate>();

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
