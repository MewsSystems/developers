using NodaTime;

namespace ExchangeRateUpdater.Core.Interfaces;

public interface IDateTimeProvider
{
    LocalDate GetCurrentDate();
    LocalDateTime GetCurrentDateTime();
    bool IsWorkingDay(LocalDate date);
    bool IsPublicationTimePassedForDate(LocalDate date);
    LocalDate GetNextBusinessDay(LocalDate date);
    LocalDate GetLastWorkingDay(LocalDate date);
}