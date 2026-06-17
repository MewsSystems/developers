namespace ExchangeRateProvider.Infrastructure.Classes;

public class FaultHandlingSettings
{
    public int MaxRetries { get; set; } = ApplicationConstants.DefaultMaxRetries;
    public int RetryDelayInMilliseconds { get; set; } = ApplicationConstants.DefaultHttpRetryDelayInMillisecond;
}