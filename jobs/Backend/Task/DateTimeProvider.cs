using System;

namespace ExchangeRateUpdater
{
    ///<inheritDoc/>
    internal class DateTimeProvider : IDateTimeProvider
    {
        ///<inheritDoc/>
        public DateTime Now() {
            return DateTime.Now;
        }
    }
}
