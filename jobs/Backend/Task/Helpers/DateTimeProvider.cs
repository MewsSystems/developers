using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.helpers
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime GetUtcDate()
        {
            return DateTime.UtcNow;
        }
    }
}
