using System.Net;
using System.Text;
using JetBrains.Annotations;

namespace ExchangeRateUpdater.Test.Support;

[UsedImplicitly]
public class HttpMessageHandlerStub : HttpMessageHandler
{
    private readonly Exception? _exception;
    private readonly string? _responseContent;

    public HttpMessageHandlerStub(Exception exception)
    {
        _exception = exception;
    }

    public HttpMessageHandlerStub(string responseContent)
    {
        _responseContent = responseContent;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (_exception is not null)
        {
            return Task.FromException<HttpResponseMessage>(_exception);
        }

        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(_responseContent ?? string.Empty, Encoding.UTF8, "application/json")
        });
    }
}