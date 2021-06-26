using System;

namespace ExchangeRateUpdater
{
    public class DateProvider : IDateProvider
    {
        public string GetCurrentDate(string format) => DateTime.Now.ToString(format);
    }
}