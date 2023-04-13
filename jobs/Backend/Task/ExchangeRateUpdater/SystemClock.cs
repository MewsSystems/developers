using ExchangeRateUpdater.Interfaces;

namespace ExchangeRateUpdater
{
    public class SystemClock : IClock
    {
        public DateOnly Today => GetCzechLocalDate();

        private DateOnly GetCzechLocalDate()
        {
            var utcNow = DateTimeOffset.UtcNow;
            var cetZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            var localDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow.DateTime, cetZone);
            return DateOnly.FromDateTime(localDateTime);
        }
    }
}
