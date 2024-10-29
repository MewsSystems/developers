using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Domain.Config
{
    public class PollyConfig
    {
        public int RetryCountAttempts { get; set; }
        public int SleepRetrySeconds { get; set; }
    }
}
