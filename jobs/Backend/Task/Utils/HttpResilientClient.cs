using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Utils;

/// <summary>
/// Retry policies and other resiliency policies can be handled in this class
/// </summary>
public class HttpResilientClient : IHttpResilientClient
{
    private readonly ILogger _logger;

    public HttpResilientClient(ILogger logger)
    {
        _logger = logger;
    }

    public async Task<HttpResponseMessage> DoGet(string url)
    {
        var _retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (exception, timeSpan, retryAttempt, context) =>
                    {
                        _logger.LogWarning($"Retry {retryAttempt}: Waiting {timeSpan.TotalSeconds} seconds before retrying due to error: {exception.Exception.Message}");
                    });

        using (var httpClient = new HttpClient())
        {
            var response = await _retryPolicy.ExecuteAsync(() =>
                httpClient.GetAsync(url));

            return response;
        }
    }
}
