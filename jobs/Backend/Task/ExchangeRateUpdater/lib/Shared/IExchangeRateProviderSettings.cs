using System;

namespace ExchangeRateUpdater.Lib.Shared
{
    public interface IExchangeRateProviderSettings
    {
        int MaxThreads { get; }
        int Precision { get; }
        int RateLimitCount { get; }
        int RateLimitDuration { get; }
        string SourceUrl { get; }
        int TimeoutSeconds { get; }
    }



}