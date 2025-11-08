using System.Net;
using System.Text;
using System.Text.Json;

namespace ExchangeRateUpdater.Tests.Services.TestHelper;

public class FakeHttpMessageHandler : HttpMessageHandler
{
    private HttpResponseMessage _response;
    private Exception _exception;
    private TimeSpan _delay = TimeSpan.Zero;
    private Func<object>? _responseFactory;

    public void SetResponse(object content)
    {
        var json = JsonSerializer.Serialize(content);
        _response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
    }

    public void SetException(Exception ex)
    {
        _exception = ex;
    }

    public void SetRawResponse(string rawJson)
    {
        _response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(rawJson, Encoding.UTF8, "application/json")
        };
    }

    public void SetDelayedResponse(TimeSpan delay)
    {
        _response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{}", Encoding.UTF8, "application/json")
        };

        _delay = delay;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (_responseFactory is not null)
        {
            var result = _responseFactory();
            if (result is Exception ex)
                throw ex;

            var json = JsonSerializer.Serialize(result);
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });
        }

        if (_exception != null)
            throw _exception;

        if (_delay > TimeSpan.Zero)
            return Task.Delay(_delay, cancellationToken).ContinueWith(_ => _response, cancellationToken);

        return Task.FromResult(_response);
    }
}
