using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
	public class HttpDataClient : IDataClient
	{
		private readonly HttpClient _httpClient;
		private readonly string _url;

		/// <summary>
		/// The constructor.
		/// </summary>
		/// <param name="httpClient">The HTTP client.</param>
		/// <param name="url">The source URL.</param>
		/// <exception cref="ArgumentNullException"><paramref name="httpClient"/> or <paramref name="url"/> is null.</exception>
		public HttpDataClient(HttpClient httpClient, string url)
		{
			_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
			_url = url ?? throw new ArgumentNullException(nameof(url));
		}

		/// <summary>
		/// Gets data from the source.
		/// </summary>
		/// <returns>The content stream.</returns>
		/// <exception cref="DataClientException">Failed to obtain data.</exception>
		public async Task<Stream> GetDataAsync()
		{
			var response = await _httpClient.GetAsync(_url).ConfigureAwait(false);
			if (response.IsSuccessStatusCode)
				return await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
			throw new DataClientException($"Failed to obtain data.\nStatus code: {response.StatusCode}\nURL: {_url}");
		}
	}
}
