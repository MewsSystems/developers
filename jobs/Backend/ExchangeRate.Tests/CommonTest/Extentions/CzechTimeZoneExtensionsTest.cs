using System;
using System.Runtime.InteropServices;
using Xunit;
using ExchangeRateUpdater.Common.Extensions;

namespace ExchangeRate.Tests.Common.Extensions
{
    public class CzechTimeZoneExtensionsTest
    {
        [Fact]
        public void GetCzechTimeZone_ReturnsCorrectTimeZone()
        {
            // Act
            var tz = CzechTimeZoneExtensions.GetCzechTimeZone();

            // Assert
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Assert.Equal("Central Europe Standard Time", tz.Id);
            else
                Assert.Equal("Europe/Prague", tz.Id);
        }

        [Fact]
        public void GetNextCzechBankUpdateUtc_ReturnsFutureTime()
        {
            // Act
            var nextUpdate = CzechTimeZoneExtensions.GetNextCzechBankUpdateUtc();

            // Assert
            Assert.True(nextUpdate > DateTimeOffset.UtcNow);
            // Should be at 14:30 Czech time
            var czechTz = CzechTimeZoneExtensions.GetCzechTimeZone();
            var local = TimeZoneInfo.ConvertTime(nextUpdate, czechTz);
            Assert.Equal(14, local.Hour);
            Assert.Equal(30, local.Minute);
        }

        [Fact]
        public void GetNextMonthUpdateUtc_ReturnsFirstDayOfNextMonthAtMidnight()
        {
            // Act
            var nextMonth = CzechTimeZoneExtensions.GetNextMonthUpdateUtc();

            // Assert
            Assert.True(nextMonth > DateTimeOffset.UtcNow);

            var czechTz = CzechTimeZoneExtensions.GetCzechTimeZone();
            var local = TimeZoneInfo.ConvertTime(nextMonth, czechTz);

            Assert.Equal(0, local.Hour);
            Assert.Equal(0, local.Minute);
            Assert.Equal(1, local.Day);
        }


        [Fact]
        public void GetNextMonthUpdateUtc_ForDecember_SetsToJanuaryNextYear()
        {
            // Arrange: Simulate a date in December
            var decemberDate = new DateTimeOffset(DateTime.UtcNow.Year, 12, 15, 10, 0, 0, TimeSpan.Zero);

            // Act: Call the logic as if today is December
            var nextMonth = CzechTimeZoneExtensions.GetNextMonthUpdateUtc(decemberDate);

            // Assert: Should be January 1st of next year at midnight Czech time
            var czechTz = CzechTimeZoneExtensions.GetCzechTimeZone();
            var local = TimeZoneInfo.ConvertTime(nextMonth, czechTz);

            Assert.Equal(1, local.Day);
            Assert.Equal(1, local.Month);
            Assert.Equal(decemberDate.Year + 1, local.Year);
            Assert.Equal(0, local.Hour);
            Assert.Equal(0, local.Minute);
        }
    }
}