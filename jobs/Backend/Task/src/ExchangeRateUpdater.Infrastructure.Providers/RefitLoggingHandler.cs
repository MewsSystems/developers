using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Infrastructure.Providers;

public class RefitLoggingHandler : DelegatingHandler
{
    private readonly ILogger<RefitLoggingHandler> _logger;

    public RefitLoggingHandler(ILogger<RefitLoggingHandler> logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Request: {request.Method} {request.RequestUri}");
        return await base.SendAsync(request, cancellationToken);
    }
}