using ExchangeRateUpdater.Caching;
using Shouldly;

namespace ExchangeRateUpdater.Tests.Caching;

public class ExchangeRateCacheHelperTests
{
    [Fact]
    public void Cnb_CommonRates_GetExpiry_ReturnsTomorrowAtMidnight()
    {
        // Arrange
        DateTime expectedDate = DateTime.Today.AddDays(1).Date;

        // Act
        DateTimeOffset actual = ExchangeRateCacheHelper.Cnb.CommonRates.GetExpiry();

        // Assert
        actual.ShouldBe(expectedDate.Date);
        actual.TimeOfDay.Ticks.ShouldBe(0);
    }

    [Fact]
    public void UncommonRates_GetExpiry_ReturnsFirstDayOfNextMonthAtMidnight()
    {
        // Arrange
        DateTime today = DateTime.Today;
        DateTime expected = new DateTime(today.Year, today.Month, 1).AddMonths(1);

        // Act
        DateTimeOffset actual = ExchangeRateCacheHelper.Cnb.UncommonRates.GetExpiry();

        // Assert
        actual.Date.ShouldBe(expected);
        actual.TimeOfDay.Ticks.ShouldBe(0);
    }
}
