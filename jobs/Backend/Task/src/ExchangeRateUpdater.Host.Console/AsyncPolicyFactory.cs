using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;
using Serilog;

namespace ExchangeRateUpdater.Host.Console;

internal class AsyncPolicyFactory
{
    private readonly ILogger _logger;

    private readonly TimeSpan[] _sleepDurations = {
        TimeSpan.FromSeconds(1),
        TimeSpan.FromSeconds(2),
        TimeSpan.FromSeconds(4),
        TimeSpan.FromSeconds(8)
    };

    public AsyncPolicyFactory(ILogger logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public IAsyncPolicy<HttpResponseMessage> CreateAsyncRetryPolicy()
    {
        return Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .Or<WebException>()
            .Or<TaskCanceledException>()
            .WaitAndRetryAsync(_sleepDurations, OnRetry);
    }

    private void OnRetry(DelegateResult<HttpResponseMessage> outcome, TimeSpan timespan, int retryAttempt, Context context)
    {
        const string messageTemplate = "Failed to execute HTTP call for context: {Context}. Got httpStatusCode: {HttpStatusCode}. Retry attempt: {Retry}. Waiting: {TimeSpan} seconds.";

        if (outcome.Exception != null)
        {
            _logger.Error(outcome.Exception, messageTemplate, context.OperationKey, outcome.Result?.StatusCode,
                retryAttempt);

        }
        else
        {
            _logger.Warning(messageTemplate, context.OperationKey, outcome.Result?.StatusCode, retryAttempt);
        }
    }
}