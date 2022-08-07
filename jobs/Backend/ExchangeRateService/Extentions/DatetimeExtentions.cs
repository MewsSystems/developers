namespace CurrencyExchangeService.Extentions
{
    static class DatetimeExtentions
    {
        public static DateTimeOffset TimeToHourMinuteSecond(this DateTime currentDate, int hour, int minute, int second)
        {
            DateTime now = DateTime.Now;
            TimeSpan difference;
            DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, minute, second);

            if (now > date)
            {
                date = date.AddDays(1);
            }

            difference = date - now;

            return DateTimeOffset.Now.AddHours(difference.Hours).AddMinutes(difference.Minutes).AddSeconds(difference.Seconds);
        }
    }
}
