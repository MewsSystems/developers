using System;

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
