using ExchangeRateUpdater.Domain.Core.Clients;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Tests.Mocks
{
	public class HttpBankClientWrapperMock : IHttpBankClientWrapper
	{
		private static HttpResponseMessage _response;

		private readonly HttpClient _client;

		public HttpBankClientWrapperMock(HttpClient client)
		{
			_client = client;
		}

		public void SetMockResponse(HttpStatusCode code, string body = null)
		{
			_response = new HttpResponseMessage();
			_response.StatusCode = code;
			_response.Content = new StringContent(body);
		}

		public async Task<HttpResponseMessage> GetAsync(string requestUri)
		{
			return await Task.FromResult(_response);
		}
	}
}
