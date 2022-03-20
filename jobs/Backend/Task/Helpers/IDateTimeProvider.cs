using System;

namespace ExchangeRateUpdater.helpers
{
    public interface IDateTimeProvider
    {
        public DateTime GetUtcDate();
    }
}