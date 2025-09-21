namespace Exchange.Infrastructure.Extensions;

public static class DateOnlyExtension
{
    public static bool IsWeekend(this DateOnly date) => date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
}