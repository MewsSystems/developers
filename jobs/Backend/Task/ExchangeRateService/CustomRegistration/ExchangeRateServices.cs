using ExchangeRateService.AutoRegistration;
using ExchangeRateService.ExternalServices;
using ExchangeRateService.Services;
using Polly;
using Polly.CircuitBreaker;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Polly.Retry;
using Polly.Timeout;

namespace ExchangeRateService.CustomRegistration;

internal class ExchangeRateServices : IServices
{
    public void Register(IServiceCollection services)
    {
        var retryPolicy = GetRetryPolicy();
        var circuitBreakerPolicy = GetCircuitBreakerPolicy();
        var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(30);
        
        services.AddTransient<IExchangeRateProvider, ExchangeRateProvider>();
        services.AddHttpClient<ICNBClientService, CNBClientService>(
                client =>
                {
                    // TODO should be from configuration - IOptions<> pattern
                    client.BaseAddress = new Uri("https://api.cnb.cz/");
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                })
            .AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(circuitBreakerPolicy)
            .AddPolicyHandler(timeoutPolicy);
    }
    
    private static readonly Action<ILogger<CNBClientService>, string, double, int, Exception?> LogRetryWarning =
        LoggerMessage.Define<string, double, int>(
            LogLevel.Warning,
            new EventId(6),
            "{Uri} delaying for {Delay}ms, then making {RetryAttemptNumber} retry attempt.");

    private static Func<IServiceProvider, HttpRequestMessage, AsyncRetryPolicy<HttpResponseMessage>> GetRetryPolicy()
        => (services, requestMessage) =>
            {
                // See recommendation by Microsoft: https://learn.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/implement-http-call-retries-exponential-backoff-polly#add-a-jitter-strategy-to-the-retry-policy
                var delay = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(1), retryCount: 5);

                return HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                    .Or<TimeoutRejectedException>()
                    .WaitAndRetryAsync(
                        delay,
                        onRetry: (delegateResult, timespan, retryAttempt, _) =>
                        {
                            var logger = services.GetService<ILogger<CNBClientService>>()!;
                            LogRetryWarning(logger, requestMessage.RequestUri!.ToString(), timespan.TotalMilliseconds, retryAttempt, delegateResult.Exception);
                        });
            };
    
    private static AsyncCircuitBreakerPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            .Or<TimeoutRejectedException>()
            .CircuitBreakerAsync(3, TimeSpan.FromSeconds(30));
    }
}