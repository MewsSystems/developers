using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
