namespace Mews.ExchangeRateMonitor.ExchangeRate.Tests.Unit.TestHelpers;

/// <summary>
/// Immitation of HttpMessageHandler that always returns the same response.
/// </summary>
/// <param name="response"></param>
public class StaticResponseHandler(HttpResponseMessage response) : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
        => Task.FromResult(response);
}
