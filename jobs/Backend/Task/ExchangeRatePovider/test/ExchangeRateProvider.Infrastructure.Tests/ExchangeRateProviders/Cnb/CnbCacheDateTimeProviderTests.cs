using ExchangeRateProvider.Infrastructure.ExchangeRateProviders.Cnb;
using Microsoft.Extensions.Time.Testing;

namespace ExchangeRateProvider.Infrastructure.Tests.ExchangeRateProviders.Cnb;

public class CnbCacheDateTimeProviderTests
{
    [Fact]
    public void GetNextFixingExpirationUtc_BeforeFixingTimeOnBusinessDay_ReturnsFixingTimeToday()
    {
        // Arrange
        // Monday, January 15, 2024, at 11:00 UTC (12:00 Prague)
        var fakeTimeProvider = new FakeTimeProvider(new DateTimeOffset(2024, 1, 15, 11, 0, 0, TimeSpan.Zero));
        var provider = new CnbCacheDateTimeProvider(fakeTimeProvider);
        var customBuffer = TimeSpan.FromMinutes(10);

        // Act
        var result = provider.GetNextFixingExpirationUtc(customBuffer);

        // Assert
        // Should return today's fixing time (14:30 Prague) + 10 minutes buffer = 13:40 UTC
        var expected = new DateTimeOffset(2024, 1, 15, 13, 40, 0, TimeSpan.Zero);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetNextFixingExpirationUtc_AfterFixingTimeOnBusinessDay_ReturnsNextBusinessDayFixing()
    {
        // Arrange
        // Monday, January 15, 2024, at 14:00 UTC (15:00 Prague)
        var fakeTimeProvider = new FakeTimeProvider(new DateTimeOffset(2024, 1, 15, 14, 0, 0, TimeSpan.Zero));
        var provider = new CnbCacheDateTimeProvider(fakeTimeProvider);
        var customBuffer = TimeSpan.FromMinutes(15);

        // Act
        var result = provider.GetNextFixingExpirationUtc(customBuffer);

        // Assert
        // Should return next business day (Tuesday, 14:45 Prague) fixing time + 15 minutes buffer
        var expected = new DateTimeOffset(2024, 1, 16, 13, 45, 0, TimeSpan.Zero);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetNextFixingExpirationUtc_OnSaturday_ReturnsNextMondayFixing()
    {
        // Arrange
        // Saturday, January 13, 2024 at 12:00 UTC (13:00 Prague)
        var fakeTimeProvider = new FakeTimeProvider(new DateTimeOffset(2024, 1, 13, 12, 0, 0, TimeSpan.Zero));
        var provider = new CnbCacheDateTimeProvider(fakeTimeProvider);
        var customBuffer = TimeSpan.FromMinutes(5);

        // Act
        var result = provider.GetNextFixingExpirationUtc(customBuffer);

        // Assert
        // Should return next Monday (January 15, 14:35 Prague) fixing time + 5 minutes buffer
        var expected = new DateTimeOffset(2024, 1, 15, 13, 35, 0, TimeSpan.Zero);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetNextFixingExpirationUtc_OnSunday_ReturnsNextMondayFixing()
    {
        // Arrange
        // Sunday, January 14, 2024 at 10:00 UTC (11:00 Prague)
        var fakeTimeProvider = new FakeTimeProvider(new DateTimeOffset(2024, 1, 14, 10, 0, 0, TimeSpan.Zero));
        var provider = new CnbCacheDateTimeProvider(fakeTimeProvider);

        // Act
        var result = provider.GetNextFixingExpirationUtc();

        // Assert
        // Should return next Monday (January 15, 14:35 Prague) fixing time + default 5 minutes buffer
        var expected = new DateTimeOffset(2024, 1, 15, 13, 35, 0, TimeSpan.Zero);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetNextFixingExpirationUtc_OnFridayAfterFixing_ReturnsNextMondayFixing()
    {
        // Arrange
        // Friday, January 19, 2024, at 15:00 UTC (16:00 Prague)
        var fakeTimeProvider = new FakeTimeProvider(new DateTimeOffset(2024, 1, 19, 15, 0, 0, TimeSpan.Zero));
        var provider = new CnbCacheDateTimeProvider(fakeTimeProvider);
        var customBuffer = TimeSpan.FromMinutes(3);

        // Act
        var result = provider.GetNextFixingExpirationUtc(customBuffer);

        // Assert
        // Should return next Monday (January 22, 14:33 Prague) fixing time + 3 minutes buffer
        var expected = new DateTimeOffset(2024, 1, 22, 13, 33, 0, TimeSpan.Zero);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("2024-01-15", 13, 29, "2024-01-15", 13, 35)] // Monday before fixing -> today + buffer
    [InlineData("2024-01-15", 13, 31, "2024-01-16", 13, 35)] // Monday after fixing -> tomorrow + buffer
    [InlineData("2024-01-16", 13, 29, "2024-01-16", 13, 35)] // Tuesday before fixing -> today + buffer
    [InlineData("2024-01-17", 13, 31, "2024-01-18", 13, 35)] // Wednesday after fixing -> tomorrow + buffer
    [InlineData("2024-01-18", 13, 29, "2024-01-18", 13, 35)] // Thursday before fixing -> today + buffer
    [InlineData("2024-01-19", 13, 31, "2024-01-22", 13, 35)] // Friday after fixing -> next Monday + buffer
    public void GetNextFixingExpirationUtc_VariousBusinessDayScenarios_ReturnsExpectedResult(
        string currentDate, int currentHour, int currentMinute,
        string expectedDate, int expectedHour, int expectedMinute)
    {
        // Arrange
        var current = DateTime.Parse(currentDate).AddHours(currentHour).AddMinutes(currentMinute);
        var fakeTimeProvider = new FakeTimeProvider(new DateTimeOffset(current, TimeSpan.Zero));
        var provider = new CnbCacheDateTimeProvider(fakeTimeProvider);
        
        var expected = DateTime.Parse(expectedDate).AddHours(expectedHour).AddMinutes(expectedMinute);
        var expectedResult = new DateTimeOffset(expected, TimeSpan.Zero);

        // Act
        var result = provider.GetNextFixingExpirationUtc();

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetNextFixingExpirationUtc_WithZeroBuffer_ReturnsExactFixingTime()
    {
        // Arrange
        // Monday, January 15, 2024, at 10:00 UTC (11:00 Prague)
        var fakeTimeProvider = new FakeTimeProvider(new DateTimeOffset(2024, 1, 15, 10, 0, 0, TimeSpan.Zero));
        var provider = new CnbCacheDateTimeProvider(fakeTimeProvider);
        var zeroBuffer = TimeSpan.Zero;

        // Act
        var result = provider.GetNextFixingExpirationUtc(zeroBuffer);

        // Assert
        // Should return exact fixing time without buffer (14:30 Prague)
        var expected = new DateTimeOffset(2024, 1, 15, 13, 30, 0, TimeSpan.Zero);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetNextFixingExpirationUtc_WithNegativeBuffer_ReturnsTimeBeforeFixing()
    {
        // Arrange
        // Monday, January 15, 2024, at 10:00 UTC (11:00 Prague)
        var fakeTimeProvider = new FakeTimeProvider(new DateTimeOffset(2024, 1, 15, 10, 0, 0, TimeSpan.Zero));
        var provider = new CnbCacheDateTimeProvider(fakeTimeProvider);
        var negativeBuffer = TimeSpan.FromMinutes(-10);

        // Act
        var result = provider.GetNextFixingExpirationUtc(negativeBuffer);

        // Assert
        // Should return fixing time minus 10 minutes (14:20 Prague)
        var expected = new DateTimeOffset(2024, 1, 15, 13, 20, 0, TimeSpan.Zero);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetNextFixingExpirationUtc_WithDefaultBuffer_Uses5MinuteBuffer()
    {
        // Arrange
        // Monday, January 15, 2024, at 10:00 UTC (11:00 Prague)
        var fakeTimeProvider = new FakeTimeProvider(new DateTimeOffset(2024, 1, 15, 10, 0, 0, TimeSpan.Zero));
        var provider = new CnbCacheDateTimeProvider(fakeTimeProvider);

        // Act
        var result = provider.GetNextFixingExpirationUtc();

        // Assert
        // Should return fixing time + default 5 minutes buffer (14:35 Prague)
        var expected = new DateTimeOffset(2024, 1, 15, 13, 35, 0, TimeSpan.Zero);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetNextFixingExpirationUtc_DuringDaylightSavingTime_HandlesTimezoneCorrectly()
    {
        // Monday, July 15, 2024, at 10:00 UTC (12:00 Prague, summer)
        var fakeTimeProvider = new FakeTimeProvider(new DateTimeOffset(2024, 7, 15, 10, 0, 0, TimeSpan.Zero));
        var provider = new CnbCacheDateTimeProvider(fakeTimeProvider);

        // Act
        var result = provider.GetNextFixingExpirationUtc();

        // Assert
        // Should return fixing time + default 5 minutes buffer (14:35 Prague)
        var expected = new DateTimeOffset(2024, 7, 15, 12, 35, 0, TimeSpan.Zero);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetNextFixingExpirationUtc_AlwaysReturnsUtcOffset()
    {
        // Arrange
        var fakeTimeProvider = new FakeTimeProvider(new DateTimeOffset(2024, 1, 15, 10, 0, 0, TimeSpan.Zero));
        var provider = new CnbCacheDateTimeProvider(fakeTimeProvider);

        // Act
        var result = provider.GetNextFixingExpirationUtc();

        // Assert
        Assert.Equal(TimeSpan.Zero, result.Offset);
    }

    [Fact]
    public void GetNextFixingExpirationUtc_WithSystemTimeProvider_UsesCurrentTime()
    {
        // Arrange - using system time provider
        var provider = new CnbCacheDateTimeProvider(TimeProvider.System);
        var beforeCall = DateTimeOffset.UtcNow;

        // Act
        var result = provider.GetNextFixingExpirationUtc();

        // Assert
        Assert.True(result > beforeCall, "Expiration should be in the future");
    }

    [Fact]
    public void GetNextFixingExpirationUtc_ConsistentResultsWithSameTimeProvider()
    {
        // Arrange - Fixed time
        var fakeTimeProvider = new FakeTimeProvider(new DateTimeOffset(2024, 1, 15, 10, 0, 0, TimeSpan.Zero));
        var provider = new CnbCacheDateTimeProvider(fakeTimeProvider);
        var buffer = TimeSpan.FromMinutes(7);

        // Act
        var result1 = provider.GetNextFixingExpirationUtc(buffer);
        var result2 = provider.GetNextFixingExpirationUtc(buffer);

        // Assert
        Assert.Equal(result1, result2);
    }
}