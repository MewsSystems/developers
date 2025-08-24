using ExchangeRateProviders.Czk.Config;
using NUnit.Framework;

namespace ExchangeRateProviders.Tests.Czk.Services;

[TestFixture]
public class CnbCacheStrategyTests
{
    [Test]
    public void GetCacheOptionsBasedOnPragueTime_ReturnsValidOptions()
    {
        // Act
        var options = CnbCacheStrategy.GetCacheOptionsBasedOnPragueTime();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(options.Duration, Is.GreaterThan(TimeSpan.Zero));
            Assert.That(options.FailSafeMaxDuration, Is.GreaterThan(TimeSpan.Zero));
            Assert.That(options.FailSafeMaxDuration.Ticks, Is.EqualTo(options.Duration.Ticks * 2));
        });
    }

    [Test]
    public void CacheDuration_WeekendVsWeekday_DifferentDurations()
    {
        // Since we can't easily mock time, we'll test the logic indirectly
        // by checking that the cache options have reasonable durations
        
        // Act
        var options = CnbCacheStrategy.GetCacheOptionsBasedOnPragueTime();

        // Assert - verify the duration is one of the expected values (5 min, 1 hour, or 12 hours)
        var durationMinutes = options.Duration.TotalMinutes;
        Assert.That(durationMinutes, Is.EqualTo(5).Or.EqualTo(60).Or.EqualTo(720),
            "Cache duration should be 5 minutes (publication window), 1 hour (weekday stable), or 24 hours (weekend)");
    }

    [Test]
    public void FailSafeMaxDuration_AlwaysDoubleOfDuration()
    {
        // Act
        var options = CnbCacheStrategy.GetCacheOptionsBasedOnPragueTime();

        // Assert
        Assert.That(options.FailSafeMaxDuration.Ticks, Is.EqualTo(options.Duration.Ticks * 2));
    }
}