using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services
{
	internal class WebRequestService : IWebRequestService
	{
		private readonly HttpClient httpClient;

		public WebRequestService(HttpClient httpClient)
		{
			this.httpClient = httpClient;
		}

		public async Task<string> GetAsStringAsync(Uri uriToGet)
		{
			return await httpClient.GetStringAsync(uriToGet).ConfigureAwait(false);
		}
	}
}
