namespace Infrastructure.Extensions
{
    public static class DateTimeExtensions
    {

        private const string PragueTimeZone = "Central European Standard Time";

        public static DateTime ToPragueDateTime(this DateTimeOffset dateTime)
        {
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(PragueTimeZone);
            DateTime dateTimeInTimeZone = TimeZoneInfo.ConvertTimeFromUtc(dateTime.UtcDateTime, timeZoneInfo);
            return dateTimeInTimeZone;
        }

    }
}
