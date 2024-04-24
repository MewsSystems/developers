namespace ExchangeRateProvider.Infrastructure.Classes;

public class HttpRetryPolicy : IHttpRetryPolicy
{
    private readonly ILogger<HttpRetryPolicy> _logger;
    private readonly AsyncPolicy<HttpResponseMessage> _cnbHttpPolicy;

    public HttpRetryPolicy(ILogger<HttpRetryPolicy> logger, IOptions<FaultHandlingSettings> faultSettings )
    {
        _logger = logger;
        int retryNo = faultSettings.Value.MaxRetries;
        int retryDelayInMilliseconds = faultSettings.Value.RetryDelayInMilliseconds;
        _cnbHttpPolicy = Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .OrInner<TimeoutException>()
            .Or<WebException>()
            .WaitAndRetryAsync( retryNo, times => TimeSpan.FromMilliseconds( retryDelayInMilliseconds ), onRetry: ( exception, _maxRetries, context ) =>
            {
                _logger.LogWarning( "Triggered retry for {PolicyKey}. Reason - {message}", context.PolicyKey, exception.Exception.Message );
            } )
            .WithPolicyKey( ApplicationConstants.CNBHttpRetryPolicyKey );
    }

    public AsyncPolicy<HttpResponseMessage> CNBHttpPolicy => _cnbHttpPolicy;
}