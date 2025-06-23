using ExchangeRateUpdater.Helpers;

namespace ExchangeRateUpdater.Tests.UnitTests
{
    public class CacheHelpersTests
    {
        [Fact]
        public void GetExpirationTimeSpan_BeforeUpdateTime_ReturnsSameDayResult()
        {
            // Arrange
            var dateTime = new DateTime(2023, 10, 1, 13, 0, 0); // 1st October 2023, 13:00
            var expectedExpiration = new TimeSpan(1, 30, 0); // Expected expiration time span
            // Act
            var result = CacheHelpers.GetExpirationTimeSpan(dateTime);
            // Assert
            Assert.Equal(expectedExpiration, result);
        }

        [Fact]
        public void GetExpirationTimeSpan_AfterUpdateTime_ReturnsNextDayResult()
        {
            // Arrange
            var dateTime = new DateTime(2023, 10, 1, 15, 0, 0); // 1st October 2023, 15:00
            var expectedExpiration = new TimeSpan(23, 30, 0); // Expected expiration time span
            // Act
            var result = CacheHelpers.GetExpirationTimeSpan(dateTime);
            // Assert
            Assert.Equal(expectedExpiration, result);
        }
    }
}
