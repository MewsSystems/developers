using ExchangeRateUpdater.Domain.Core.Clients;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Domain.Logic.Clients
{
	public class HttpBankClientWrapper : IHttpBankClientWrapper
	{
		private readonly HttpClient _client;

		public HttpBankClientWrapper(HttpClient client)
		{
			_client = client;
		}

		public async Task<HttpResponseMessage> GetAsync(string requestUri)
		{
			return await this._client.GetAsync(requestUri);
		}
	}
}
