namespace Adpater.Http.CzechNationalBank.Test.Fakes
{
    public class FakeHttpMessageHandler : DelegatingHandler
    {
        private HttpResponseMessage _fakeResponse;

        public void SetReponse(HttpResponseMessage fakeResponse)
        {
            _fakeResponse = fakeResponse;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_fakeResponse);
        }
    }
}
