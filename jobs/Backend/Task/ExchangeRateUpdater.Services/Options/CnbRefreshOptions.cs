using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services.Options
{
    public class CnbRefreshOptions
    {
        public string TimeZoneId { get; set; }
        public TimeSpan FirstAttemptAt { get; set; }
        public TimeSpan CutoffAt { get; set; }
        public TimeSpan RetryInterval { get; set; }
    }
}
