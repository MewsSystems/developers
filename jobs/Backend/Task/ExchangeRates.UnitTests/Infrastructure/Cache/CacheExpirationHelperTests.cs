using ExchangeRates.Infrastructure.Cache;
using FluentAssertions;
namespace ExchangeRates.UnitTests.Infrastructure.Cache
{
    public class CacheExpirationHelperTests
    {
        [Fact]
        public void GetCacheExpirationToNextCzTime_BeforeExpiration_ReturnsPositiveTimeSpan()
        {
            // Arrange
            var czTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            var nowCz = new DateTime(2025, 10, 31, 12, 0, 0); // 12:00 CZ
            var nowUtc = TimeZoneInfo.ConvertTimeToUtc(nowCz, czTimeZone);

            var expirationTime = new TimeOnly(14, 0); // 14:00 CZ

            // Act
            var timespan = CacheExpirationHelper.GetCacheExpirationToNextCzTime(expirationTime, nowUtc);

            // Assert
            timespan.Should().Be(TimeSpan.FromHours(2)); // ~2h until 14:00 CZ
        }

        [Fact]
        public void GetCacheExpirationToNextCzTime_AfterExpiration_ReturnsTimeUntilTomorrow()
        {
            // Arrange
            var czTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            var nowCz = new DateTime(2025, 10, 31, 15, 0, 0); // 15:00 CZ
            var nowUtc = TimeZoneInfo.ConvertTimeToUtc(nowCz, czTimeZone);

            var expirationTime = new TimeOnly(14, 0); // 14:00 CZ

            // Act
            var timespan = CacheExpirationHelper.GetCacheExpirationToNextCzTime(expirationTime, nowUtc);

            // Assert
            timespan.Should().Be(TimeSpan.FromHours(23)); // ~23h until tomorrow 14:00 CZ
        }

        [Fact]
        public void GetCacheExpirationToNextCzTime_ExactlyAtExpiration_ReturnsTimeUntilTomorrow()
        {
            // Arrange
            var czTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            var nowCz = new DateTime(2025, 10, 31, 14, 0, 0); // 14:00 CZ
            var nowUtc = TimeZoneInfo.ConvertTimeToUtc(nowCz, czTimeZone);

            var expirationTime = new TimeOnly(14, 0); // 14:00 CZ

            // Act
            var timespan = CacheExpirationHelper.GetCacheExpirationToNextCzTime(expirationTime, nowUtc);

            // Assert
            timespan.Should().Be(TimeSpan.FromHours(24)); // ~24h until tomorrow 14:00 CZ
        }
    }
}