using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Data.Models
{
    public class RetryConfiguration
    {
        public int Retries { get; }

        public TimeSpan WaitSeconds { get; }

        public TimeSpan TimeoutSeconds { get; }
    }
}
