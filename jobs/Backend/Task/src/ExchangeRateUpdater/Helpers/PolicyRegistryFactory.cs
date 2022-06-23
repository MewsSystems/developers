using Polly;
using Serilog;

namespace ExchangeRateUpdater.Helpers;

internal class PolicyCreator
{
    private readonly TimeSpan[] _sleepDurations;
    private readonly ILogger _logger;

    public PolicyCreator(TimeSpan[] retrySleepDurations, ILogger logger)
    {
        _sleepDurations = retrySleepDurations;
        _logger         = logger;
    }

    public IAsyncPolicy<HttpResponseMessage> CreateAsyncRetryPolicy()
    {
        return Policy<HttpResponseMessage>.Handle<HttpRequestException>()
                                          .Or<TaskCanceledException>()
                                          .WaitAndRetryAsync(_sleepDurations, OnRetry);
    }

    private void OnRetry(DelegateResult<HttpResponseMessage> outcome, TimeSpan timespan, int retryAttempt,
                         Context context)
    {
        const string messageTemplate = "Http call failed for {Context} with {HttpStatusCode}. Retry attempt: {Retry}";

        if (outcome.Exception != null)
            _logger.Error(outcome.Exception, messageTemplate, context.OperationKey, outcome.Result?.StatusCode, retryAttempt);
        else
            _logger.Warning(messageTemplate, context.OperationKey, outcome.Result?.StatusCode, retryAttempt);
    }
}