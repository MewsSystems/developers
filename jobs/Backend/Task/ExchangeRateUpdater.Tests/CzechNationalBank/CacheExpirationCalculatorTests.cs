using ExchangeRateUpdater.RateSources.CzechNationalBank;
using Microsoft.Extensions.Time.Testing;

namespace ExchangeRateUpdater.Tests.CzechNationalBank;

public class CacheExpirationCalculatorTests
{
    private readonly FakeTimeProvider _timeProvider;
    private readonly CzechNationalBankRatesCacheExpirationCalculator _calculator;

    public CacheExpirationCalculatorTests()
    {
        _timeProvider = new FakeTimeProvider();
        _calculator = new CzechNationalBankRatesCacheExpirationCalculator(_timeProvider);
    }

    [Fact]
    public void GivenPrimaryDateCalculation_WhenDateInFuture_ShouldThrowException()
    {
        _timeProvider.SetUtcNow(new DateTime(2025, 01, 25));
        var targetDate = new DateOnly(2025, 01, 26);

        var action = () => _calculator.GetPrimaryRateExpirationDate(targetDate);
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenPrimaryDateCalculation_WhenDateIsTodayAndNonWorkingDay_ShouldSetExpirationDateToNextWorkingDay()
    {
        _timeProvider.SetUtcNow(new DateTime(2025, 01, 26)); // Sunday
        var targetDate = new DateOnly(2025, 01, 26);

        var result = _calculator.GetPrimaryRateExpirationDate(targetDate);

        result.Should().Be(new DateTime(2025, 01, 27, 13, 30, 0)); // 14:30 CET
    }

    [Fact]
    public void GivenPrimaryDateCalculation_WhenTargetIsToday_AndWorkingDay_AndBeforeUpdateDate_ShouldSetExpirationDateToUpdateDate()
    {
        _timeProvider.SetUtcNow(new DateTime(2025, 01, 27, 10, 0, 0)); // Monday
        var targetDate = new DateOnly(2025, 01, 27);

        var result = _calculator.GetPrimaryRateExpirationDate(targetDate);

        result.Should().Be(new DateTime(2025, 01, 27, 13, 30, 0));
    }

    [Fact]
    public void GivenPrimaryDateCalculation_WhenTargetIsToday_AndWorkingDay_AndAfterUpdateTime_ShouldSetExpirationDateToNextDayUpdateDate()
    {
        _timeProvider.SetUtcNow(new DateTime(2025, 01, 27, 16, 0, 0)); // Monday
        var targetDate = new DateOnly(2025, 01, 27);

        var result = _calculator.GetPrimaryRateExpirationDate(targetDate);

        result.Should().Be(new DateTime(2025, 01, 28, 13, 30, 0));
    }

    [Fact]
    public void GivenPrimaryDateCalculation_WhenTargetIsInPast_AndWorkingDay_ShouldReturnNull()
    {
        _timeProvider.SetUtcNow(new DateTime(2025, 01, 27));

        var targetDate = new DateOnly(2025, 01, 24); // Friday
        var result = _calculator.GetPrimaryRateExpirationDate(targetDate);

        result.Should().Be(null);
    }


    [Fact]
    public void GivenPrimaryDateCalculation_WhenTargetIsInPast_AndTargetNonWorkingDay_AndNextWorkingDayAfterTargetDateIsInPast_ShouldReturnNull()
    {
        _timeProvider.SetUtcNow(new DateTime(2025, 01, 27));

        var targetDate = new DateOnly(2025, 01, 18); // Saturday
        var result = _calculator.GetPrimaryRateExpirationDate(targetDate);

        result.Should().Be(null);
    }

    [Fact]
    public void GivenPrimaryDateCalculation_WhenTargetIsInPast_AndTargetIsNonWorkingDay_AndNextWorkingDayAfterTargetDateIsInFuture_ShouldReturnNextWorkingDayUpdateTime()
    {
        _timeProvider.SetUtcNow(new DateTime(2025, 01, 26)); // Sunday

        var targetDate = new DateOnly(2025, 01, 25); // Saturday
        var result = _calculator.GetPrimaryRateExpirationDate(targetDate);

        result.Should().Be(new DateTime(2025, 01, 27, 13, 30, 0));
    }

    [Fact]
    public void GivenPrimaryDateCalculation_WhenTargetIsInPast_AndTargetIsNonWorkingDay_AndNextWorkingDayAfterTargetIsToday_AndNowIsBeforeUpdateTime_ShouldSetExpirationToTodayUpdateTime()
    {
        _timeProvider.SetUtcNow(new DateTime(2025, 01, 27, 10, 0, 0)); // Monday

        var targetDate = new DateOnly(2025, 01, 25); // Saturday
        var result = _calculator.GetPrimaryRateExpirationDate(targetDate);

        result.Should().Be(new DateTime(2025, 01, 27, 13, 30, 0));
    }

    [Fact]
    public void GivenPrimaryDateCalculation_WhenTargetIsInPast_AndTargetIsNonWorkingDay_AndNextWorkingDayAfterTargetIsToday_AndNowIsAfterUpdateTime_ShouldSetExpirationToNextWorkingDayUpdateTime()
    {
        _timeProvider.SetUtcNow(new DateTime(2025, 01, 27, 15, 0, 0)); // Monday

        var targetDate = new DateOnly(2025, 01, 25); // Saturday
        var result = _calculator.GetPrimaryRateExpirationDate(targetDate);

        result.Should().Be(new DateTime(2025, 01, 28, 13, 30, 0));
    }
}