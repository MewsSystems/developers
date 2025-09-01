using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services.Time
{
    public class AppClock : IAppClock
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
