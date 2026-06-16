using Polly;

namespace Adapter.Http.CNB.Resiliency;

public static class HttpRequestResiliency
{
    public static async Task<HttpResponseMessage> SendWithRetryAsync(this HttpClient httpClient,
        Func<Task<HttpResponseMessage>> requestFunc, int retryCount = 3)
    {
        var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .Or<TimeoutException>()
            .WaitAndRetry(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        return await retryPolicy.Execute(requestFunc);
    }
}