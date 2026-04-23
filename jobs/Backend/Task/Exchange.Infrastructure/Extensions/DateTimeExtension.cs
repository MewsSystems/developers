namespace Exchange.Infrastructure.Extensions;

public static class DateTimeExtension
{
    public static DateOnly ToDateOnly(this DateTime dateTime) => DateOnly.FromDateTime(dateTime);
}