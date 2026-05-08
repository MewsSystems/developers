using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace ExchangeRates.Api.Infrastructure.Clients.Common.DelegatingHandlers;

[ExcludeFromCodeCoverage]
public class RequestTimingDelegatingHandler : DelegatingHandler
{
    private readonly ILogger<RequestTimingDelegatingHandler> _logger;

    public RequestTimingDelegatingHandler(ILogger<RequestTimingDelegatingHandler> logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            var response = await base.SendAsync(request!, cancellationToken);
            sw.Stop();
            _logger.LogInformation("Got {StatusCode} response from {Uri} in {ElapsedMilliseconds} ms", (int)response.StatusCode, request?.RequestUri, sw.ElapsedMilliseconds);
            return response;
        }
        catch (Exception ex)
        {
            sw.Stop();
            _logger.LogError(ex, "Got an error calling {Uri} in {ElapsedMilliseconds} ms. Error is {ErrorMsg}", request?.RequestUri, sw.ElapsedMilliseconds, ex.Message);
            throw;
        }
    }
}
