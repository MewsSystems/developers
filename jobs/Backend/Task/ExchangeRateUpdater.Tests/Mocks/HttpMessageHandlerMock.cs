using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Tests.Mocks
{
	public class HttpMessageHandlerMock : HttpMessageHandler
	{
		private static HttpResponseMessage _response;

		public void SetMockResponse(HttpStatusCode code, string body = null)
		{
			_response = new HttpResponseMessage();
			_response.StatusCode = code;
			_response.Content = new StringContent(body);
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			return await Task.FromResult(_response);
		}
	}
}
