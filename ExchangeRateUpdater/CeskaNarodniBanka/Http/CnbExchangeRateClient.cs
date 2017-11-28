namespace CeskaNarodniBanka.Http {
	using System.Net.Http;
	using System.Net.Http.Formatting;
	using System.Runtime.Serialization;
	using System.Threading.Tasks;
	using System.Xml.Serialization;
	using CeskaNarodniBanka.Http.Configuration;
	using ExchangeRateUpdater.Diagnostics;

	public class CnbExchangeRateClient : ICnbExchangeRateClient {
		private static readonly string requestUriString = "denni_kurz.xml";
		private HttpClient _httpClient;

		public CnbExchangeRateClient(HttpClient httpClient, ICnbHttpClientConfiguration httpConfig) {
			_httpClient = Ensure.IsNotNull(httpConfig, nameof(httpConfig))
				.Configure(Ensure.IsNotNull(httpClient, nameof(httpClient)));
		}

		public async Task<CnbExchangeRateRoot> GetAsync() {
			var response = await _httpClient.GetAsync(requestUriString);

			CnbExchangeRateRoot result;

			using (var stream = await response.EnsureSuccessStatusCode().Content.ReadAsStreamAsync()) {
				var serializer = new XmlSerializer(typeof(CnbExchangeRateRoot), "");

				result = (CnbExchangeRateRoot)serializer.Deserialize(stream);
			}

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
