namespace ExchangeRateUpdater.Providers.Tests.Fakes
{
    public class HttpMessageHandlerFake : DelegatingHandler
    {
        private readonly HttpResponseMessage _fakeResponse;

        public HttpMessageHandlerFake(HttpResponseMessage responseMessage)
        {
            _fakeResponse = responseMessage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_fakeResponse);
        }
    }
}