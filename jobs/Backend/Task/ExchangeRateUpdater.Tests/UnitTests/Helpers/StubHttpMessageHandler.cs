using System.Net;
using System.Text.Json;
using ExchangeRateUpdater.Repositories;

namespace ExchangeRateUpdater.Tests.UnitTests.Helpers;

public class StubHttpMessageHandler : HttpMessageHandler
{
    private readonly List<RateResource> _rateResources;

    public StubHttpMessageHandler(List<RateResource> rateResources)
    {
        _rateResources = rateResources;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var rate = new RateResource()
        {
            Amount = 50,
            CurrencyCode = "NZD",
            Rate = 23M
        };
        var result = new DailyRatesResponse(_rateResources);
        return Task.FromResult(CreateHttpResponseMessage(HttpStatusCode.OK, result));
    }
    
    private HttpResponseMessage CreateHttpResponseMessage(HttpStatusCode statusCode, object content)
    {
        var json = JsonSerializer.Serialize(content);
        var response = new HttpResponseMessage(statusCode);
        response.Content = new StringContent(json);
        return response;
    }
}