using Polly;

namespace ExchangeRateUpdater.Helpers;

internal class PolicyCreator
{
    private readonly TimeSpan[] _sleepDurations;

    public PolicyCreator(TimeSpan[] retrySleepDurations)
    {
        _sleepDurations = retrySleepDurations;
    }

    public IAsyncPolicy<HttpResponseMessage> CreateAsyncRetryPolicy()
    {
        return Policy<HttpResponseMessage>.Handle<HttpRequestException>()
                                          .Or<TaskCanceledException>()
                                          .WaitAndRetryAsync(_sleepDurations,
                                                             onRetry: (outcome, timespan, retryAttempt, context) => 
                                                             { 
                                                                 //Log
                                                             });
    }
}