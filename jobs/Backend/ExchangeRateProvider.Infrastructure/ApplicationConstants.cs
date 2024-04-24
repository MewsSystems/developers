namespace ExchangeRateProvider.Infrastructure;

public static class ApplicationConstants
{
    public const int DefaultMaxRetries = 3;
    public const int DefaultHttpRetryDelayInMillisecond = 500;
    public const string CNBHttpRetryPolicyKey = "CNBPolicyKey";
}