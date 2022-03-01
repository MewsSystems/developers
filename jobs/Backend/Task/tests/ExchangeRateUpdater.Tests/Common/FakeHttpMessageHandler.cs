using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Tests.Common;

public class FakeHttpMessageHandler : DelegatingHandler
{
    private HttpResponseMessage _fakeResponse;

    public FakeHttpMessageHandler(HttpResponseMessage responseMessage)
    {
        _fakeResponse = responseMessage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(_fakeResponse);
    }
}