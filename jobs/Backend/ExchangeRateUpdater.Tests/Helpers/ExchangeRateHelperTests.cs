using ExchangeRateUpdater.Helpers;
using Microsoft.Extensions.Time.Testing;

namespace ExchangeRateUpdater.Tests.Helpers;

[TestFixture]
public class ExchangeRateHelperTests
{
    [Test]
    public void GetTimeUntilNextExchangeRate_ReturnsCorrectTime_WhenBeforeNextUpdate()
    {
        // Arrange
        var fakeTimeProvider = new FakeTimeProvider(new DateTime(2023, 10, 10, 14, 0, 0, DateTimeKind.Utc));
        ExchangeRateHelper.TimeProvider = fakeTimeProvider;
        const string expected = "Time until next exchange rate update: 0 hours and 31 minutes.";
            
        // Act
        var result = ExchangeRateHelper.GetTimeUntilNextExchangeRateData();

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void GetTimeUntilNextExchangeRate_ReturnsCorrectTime_WhenExactlyAtUpdateTime()
    {
        // Arrange
        var fakeTimeProvider = new FakeTimeProvider(new DateTime(2023, 10, 10, 14, 31, 0, DateTimeKind.Utc));
        ExchangeRateHelper.TimeProvider = fakeTimeProvider;
        const string expected = "Time until next exchange rate update: 24 hours and 0 minutes.";
        
        // Act
        var result = ExchangeRateHelper.GetTimeUntilNextExchangeRateData();
        
        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void GetTimeUntilNextExchangeRate_ReturnsCorrectTime_WhenAfterNextUpdate()
    {
        // Arrange
        var fakeTimeProvider = new FakeTimeProvider(new DateTime(2023, 10, 10, 15, 0, 0, DateTimeKind.Utc));
        ExchangeRateHelper.TimeProvider = fakeTimeProvider;
        const string expected = "Time until next exchange rate update: 23 hours and 31 minutes.";
            
        // Act
        var result = ExchangeRateHelper.GetTimeUntilNextExchangeRateData();

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }
        
    [Test]
    public void GetTimeUntilNextExchangeRate_ReturnsCorrectTime_WhenOnFridayAfterUpdate()
    {
        // Arrange
        var fakeTimeProvider = new FakeTimeProvider(new DateTime(2023, 10, 13, 15, 0, 0, DateTimeKind.Utc));
        ExchangeRateHelper.TimeProvider = fakeTimeProvider;
        const string expected = "Time until next exchange rate update: 71 hours and 31 minutes.";
            
        // Act
        var result = ExchangeRateHelper.GetTimeUntilNextExchangeRateData();

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }
        
    [Test]
    public void GetTimeUntilNextExchangeRate_ReturnsCorrectTime_WhenOnSaturday()
    {
        // Arrange
        var fakeTimeProvider = new FakeTimeProvider(new DateTime(2023, 10, 14, 10, 0, 0, DateTimeKind.Utc));
        ExchangeRateHelper.TimeProvider = fakeTimeProvider;
        const string expected = "Time until next exchange rate update: 52 hours and 31 minutes.";
            
        // Act
        var result = ExchangeRateHelper.GetTimeUntilNextExchangeRateData();
            
        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }
        
    [Test]
    public void GetTimeUntilNextExchangeRate_ReturnsCorrectTime_WhenOnSunday()
    {
        // Arrange
        var fakeTimeProvider = new FakeTimeProvider(new DateTime(2023, 10, 15, 10, 0, 0, DateTimeKind.Utc));
        ExchangeRateHelper.TimeProvider = fakeTimeProvider;
        const string expected = "Time until next exchange rate update: 28 hours and 31 minutes.";
            
        // Act
        var result = ExchangeRateHelper.GetTimeUntilNextExchangeRateData();
            
        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }
}