using System;
using Xunit;

namespace ExchangeRateUpdater
{
    public class MonthlyRateHelperTests
    {
        [Theory]
        // Regular middle-of-month
        [InlineData("2025-11-15T00:00:00Z", "2025-10")]
        // Start of the month 
        [InlineData("2025-11-01T00:00:00Z", "2025-10")]
        // Start of November (when converted to CEST)
        [InlineData("2025-10-31T23:00:00Z", "2025-10")]
        // End of November (when converted to CEST)
        [InlineData("2025-11-30T22:59:59Z", "2025-10")]
        // Start of December (when converted to CEST)
        [InlineData("2025-11-30T23:00:00Z", "2025-11")]
        // January → previous year
        [InlineData("2025-01-10T12:00:00Z", "2024-12")]
        // Leap year: March after February 29
        [InlineData("2024-03-01T00:00:00Z", "2024-02")]
        public void GetDeclarationMonth_ReturnsExpectedMonth(string utcString, string expected)
        {
            // Arrange
            var utcDate = DateTime.Parse(utcString, null, System.Globalization.DateTimeStyles.AdjustToUniversal);

            // Act
            var result = MonthlyRateHelper.GetDeclarationMonth(utcDate);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}