using System;

namespace ExchangeRateUpdater.Data.Models
{
    public class RetryConfiguration
    {
        public RetryConfiguration(int retries, int waitSeconds, int timeOutSeconds)
        {
            Retries = retries;
            WaitSeconds = TimeSpan.FromSeconds(waitSeconds);
            TimeoutSeconds = TimeSpan.FromSeconds(timeOutSeconds);
        }

        public int Retries { get; }

        public TimeSpan WaitSeconds { get; }

        public TimeSpan TimeoutSeconds { get; }
    }
}
