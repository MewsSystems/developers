using Exchange.Infrastructure.DateTimeProviders;
using Exchange.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;

namespace Exchange.Infrastructure.ApiClients;

public interface ICnbApiClientDataUpdateCalculator
{
    DateTime GetNextExpectedUpdateDate(DateOnly lastUpdate);
}

public class CnbApiClientDataUpdateCalculator(
    IDateTimeProvider dateTimeProvider,
    ILogger<CnbApiClientDataUpdateCalculator> logger
) : ICnbApiClientDataUpdateCalculator
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
            logger.LogInformation("Data update is expected on the same day.");
            return targetDate.ToDateTime(DataUpdateTime);
        }

        logger.LogInformation("Data update is expected on the next working day.");
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