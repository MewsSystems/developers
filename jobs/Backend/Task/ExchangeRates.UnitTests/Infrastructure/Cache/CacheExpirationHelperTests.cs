using ExchangeRates.Infrastructure.Cache;
using FluentAssertions;

namespace ExchangeRates.UnitTests.Infrastructure.Cache
{
    public class CacheExpirationHelperTests
    {
        [Fact]
        public void GetCacheExpirationToNextCzTime_BeforeRefresh_ReturnsSameDayTimeSpan()
        {
            // Arrange: Current time is before daily refresh on a weekday
            var czTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            var nowCz = new DateTime(2025, 10, 31, 12, 0, 0); // Friday 12:00 CZ
            var nowUtc = TimeZoneInfo.ConvertTimeToUtc(nowCz, czTimeZone);

            var dailyRefreshTime = new TimeOnly(14, 0); // 14:00 CZ
            var lastUpdateDate = new DateTime(2025, 10, 30);

            // Act
            var timespan = CacheExpirationHelper.GetCacheExpirationToNextCzTime(
                dailyRefreshTime,
                lastUpdateDate,
                nowUtc
            );

            // Assert: 2 hours until next refresh
            timespan.Should().Be(TimeSpan.FromHours(2));
        }

        [Fact]
        public void GetCacheExpirationToNextCzTime_AfterRefresh_ReturnsNextDayDayTimeSpan()
        {
            // Arrange: Current time is before daily refresh on a weekday
            var czTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            var nowCz = new DateTime(2025, 10, 30, 15, 0, 0); // Thursday 15:00 CZ
            var nowUtc = TimeZoneInfo.ConvertTimeToUtc(nowCz, czTimeZone);

            var dailyRefreshTime = new TimeOnly(14, 0); // 14:00 CZ
            var lastUpdateDate = new DateTime(2025, 10, 30);

            // Act
            var timespan = CacheExpirationHelper.GetCacheExpirationToNextCzTime(
                dailyRefreshTime,
                lastUpdateDate,
                nowUtc
            );

            // Assert: 2 hours until next refresh
            timespan.Should().Be(TimeSpan.FromHours(23));
        }

        [Fact]
        public void GetCacheExpirationToNextCzTime_DelayedRefresh_ReturnsExtraMinutesTimeSpan()
        {
            // Arrange: Current time is after daily refresh on a weekday
            var czTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            var nowCz = new DateTime(2025, 10, 31, 14, 15, 0); // Friday 14:15 CZ
            var nowUtc = TimeZoneInfo.ConvertTimeToUtc(nowCz, czTimeZone);

            var dailyRefreshTime = new TimeOnly(14, 0); // 14:00 CZ
            var lastUpdateDate = new DateTime(2025, 10, 30);

            // Act
            var timespan = CacheExpirationHelper.GetCacheExpirationToNextCzTime(
                dailyRefreshTime,
                lastUpdateDate,
                nowUtc
            );

            // Assert: 5 minutes until next refresh
            timespan.Should().Be(TimeSpan.FromMinutes(5));
        }

        [Fact]
        public void GetCacheExpirationToNextCzTime_AfterRefresh_OnWeekday_ReturnsTimeUntilNextBusinessDay()
        {
            // Arrange: Current time is after daily refresh on Friday
            var czTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            var nowCz = new DateTime(2025, 10, 31, 15, 0, 0); // Friday 15:00 CZ
            var nowUtc = TimeZoneInfo.ConvertTimeToUtc(nowCz, czTimeZone);

            var dailyRefreshTime = new TimeOnly(14, 0); // 14:00 CZ
            var lastUpdateDate = new DateTime(2025, 10, 31); // Last update same day

            // Act
            var timespan = CacheExpirationHelper.GetCacheExpirationToNextCzTime(
                dailyRefreshTime,
                lastUpdateDate,
                nowUtc
            );

            // Assert: Next refresh is Monday 3 Nov 14:00
            var nextRefreshCz = new DateTime(2025, 11, 3, 14, 0, 0);
            var expected = nextRefreshCz - nowCz;

            timespan.Should().Be(expected);
        }

        [Fact]
        public void GetCacheExpirationToNextCzTime_OnSaturday_ReturnsTimeUntilNextBusinessDay()
        {
            // Arrange: Current time is on Saturday
            var czTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            var nowCz = new DateTime(2025, 11, 1, 10, 0, 0); // Saturday 10:00 CZ
            var nowUtc = TimeZoneInfo.ConvertTimeToUtc(nowCz, czTimeZone);

            var dailyRefreshTime = new TimeOnly(14, 0); // 14:00 CZ
            var lastUpdateDate = new DateTime(2025, 10, 31); // Last update was Friday

            // Act
            var timespan = CacheExpirationHelper.GetCacheExpirationToNextCzTime(
                dailyRefreshTime,
                lastUpdateDate,
                nowUtc
            );

            // Assert: Next refresh is Monday 3 Nov 14:00
            var nextRefreshCz = new DateTime(2025, 11, 3, 14, 0, 0);
            var expected = nextRefreshCz - nowCz;

            timespan.Should().Be(expected);
        }
    }
}
