using System.Net;

namespace ExchangeRateProviderAPI_PaolaRojas.UnitTests.Mocks
{
    public class MockHttpMessageHandler : HttpMessageHandler
    {
        private string? _mockContent;
        private Exception? _exception;

        public void SetMockResponse(string content)
        {
            _mockContent = content;
            _exception = null;
        }

        public void SetException(Exception ex)
        {
            _exception = ex;
            _mockContent = null;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_exception is not null)
            {
                throw _exception;
            }

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(_mockContent ?? "")
            };

            return Task.FromResult(response);
        }
    }
}