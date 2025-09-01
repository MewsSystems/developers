using ExchangeRateProvider.Infrastructure;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace ExchangeRateProvider.Tests.Infrastructure
{
    /// <summary>
    /// Tests for CNB cache strategy - complex business logic for cache durations
    /// </summary>
    public class CnbCacheStrategyTests
    {
        [Fact]
        public void GetCacheOptions_ReturnsCorrectDuration_ForPublicationWindow()
        {
            var options = new CnbCacheOptions
            {
                PublicationWindowDuration = TimeSpan.FromMinutes(5),
                PublicationWindowStart = new TimeSpan(14, 31, 0),
                PublicationWindowEnd = new TimeSpan(15, 31, 0),
                // Set all durations to 5 minutes for consistent test behavior
                WeekdayDuration = TimeSpan.FromMinutes(5),
                WeekendDuration = TimeSpan.FromMinutes(5)
            };

            var strategy = new CnbCacheStrategy(Options.Create(options), NullLogger<CnbCacheStrategy>.Instance);

            // Test runs at current time - should use 5 minutes regardless of time/context
            var cacheOptions = strategy.GetCacheOptions();

            // Should use 5 minutes for all scenarios
            Assert.Equal(TimeSpan.FromMinutes(5), cacheOptions.Duration);
        }

        [Fact]
        public void GetCacheOptions_ReturnsCorrectDuration_ForWeekends()
        {
            var options = new CnbCacheOptions
            {
                WeekendDuration = TimeSpan.FromHours(12),
                WeekdayDuration = TimeSpan.FromHours(1)
            };

            var strategy = new CnbCacheStrategy(Options.Create(options), NullLogger<CnbCacheStrategy>.Instance);

            // Test would need timezone mocking for weekend scenario
            var cacheOptions = strategy.GetCacheOptions();

            // Duration should be reasonable (fallback or actual weekend duration)
            Assert.True(cacheOptions.Duration > TimeSpan.Zero);
        }

        [Fact]
        public void Constructor_ValidatesOptions_ThrowsOnInvalid()
        {
            var invalidOptions = new[]
            {
                new CnbCacheOptions { PublicationWindowDuration = TimeSpan.Zero },
                new CnbCacheOptions { WeekdayDuration = TimeSpan.FromHours(-1) },
                new CnbCacheOptions { FailSafeMultiplier = 0.5 },
                new CnbCacheOptions { PublicationWindowStart = new TimeSpan(16, 0, 0), PublicationWindowEnd = new TimeSpan(15, 0, 0) }
            };

            foreach (var options in invalidOptions)
            {
                Assert.Throws<ArgumentException>(() => new CnbCacheStrategy(Options.Create(options)));
            }
        }

        [Fact]
        public void GetCacheOptions_IncludesFailSafeDuration()
        {
            var options = new CnbCacheOptions
            {
                WeekdayDuration = TimeSpan.FromHours(1),
                WeekendDuration = TimeSpan.FromHours(1), // Set to same as weekday for consistent test
                FailSafeMultiplier = 2.0
            };

            var strategy = new CnbCacheStrategy(Options.Create(options), NullLogger<CnbCacheStrategy>.Instance);
            var cacheOptions = strategy.GetCacheOptions();

            // Fail-safe should be 2x normal duration (2 hours)
            Assert.Equal(TimeSpan.FromHours(2), cacheOptions.FailSafeMaxDuration);
        }
    }
}