using Exchange.Infrastructure.DateTimeProviders;
using Exchange.Infrastructure.Extensions;

namespace Exchange.Infrastructure.ApiClients;

public interface ICnbApiClientDataUpdateCalculator
{
    DateTime GetNextExpectedUpdateDate(DateOnly lastUpdate);
}

public class CnbApiClientDataUpdateCalculator(IDateTimeProvider dateTimeProvider) : ICnbApiClientDataUpdateCalculator
{
    private static readonly TimeOnly DataUpdateTime = new(14, 30);

    private readonly DateOnly[] _bankHolidays =
    [
        new(2025, 11, 1),
        new(2025, 12, 6),
        new(2025, 12, 8),
        new(2025, 12, 25)
    ];


    public DateTime GetNextExpectedUpdateDate(DateOnly lastUpdate)
    {
        var targetDate = lastUpdate;

        if (CanUpdateOnSameDay(targetDate, dateTimeProvider.Now))
        {
            return targetDate.ToDateTime(DataUpdateTime);
        }

        return GetNextWorkingDate(targetDate).ToDateTime(DataUpdateTime);
    }

    private bool CanUpdateOnSameDay(DateOnly date, DateTime currentDateTime) =>
        IsWorkingDay(date) &&
        date == currentDateTime.ToDateOnly() &&
        currentDateTime.TimeOfDay < DataUpdateTime.ToTimeSpan();

    private bool IsWorkingDay(DateOnly date) =>
        !date.IsWeekend() && !IsHoliday(date);

    private bool IsHoliday(DateOnly date) => _bankHolidays.Contains(date);

    private DateOnly GetNextWorkingDate(DateOnly startDate)
    {
        var nextDate = startDate.AddDays(1);
        while (!IsWorkingDay(nextDate))
        {
            nextDate = nextDate.AddDays(1);
        }

        return nextDate;
    }
}