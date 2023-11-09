using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Data.Models
{
    public class RetryConfiguration
    {
        public int Retries { get; set; }

        public TimeSpan WaitSeconds { get; set;  }

        public TimeSpan TimeoutSeconds { get; set; }
    }
}
