using AutoFixture.Xunit2;
using ExchangeRateUpdater.Contracts;
using ExchangeRateUpdater.DataSources.RefreshSchedule;
using Moq;

namespace ExchangeRateUpdaterTests;

public class DefaultRateRefreshSchedulerTests
{
    [Theory, AutoData]
    public void ReturnCurrentDateIfRefreshTimeHasNotPassed(Mock<IDateTimeService> dateTimeServiceMock)
    {
        var utcNow = new DateTime(2025, 11, 9, 12, 45, 0);
        var today = utcNow.Date;

        dateTimeServiceMock.Setup(s => s.GetUtcNow()).Returns(utcNow);
        dateTimeServiceMock.Setup(s => s.GetToday()).Returns(today);

        var scheduler = new DefaultRateRefreshScheduler(
            new TimeOnly(14, 30),
            TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"),
            dateTimeServiceMock.Object);

        var expectedRefreshTime = new DateTime(2025, 11, 9, 13, 30, 0);
        var result = scheduler.GetNextRefreshTime();

        Assert.Equal(expectedRefreshTime, result.DateTime);
    }

    [Theory, AutoData]
    public void ReturnNextDateIfRefreshTimeHasPassed(Mock<IDateTimeService> dateTimeServiceMock)
    {
        var utcNow = new DateTime(2025, 11, 9, 13, 45, 0);
        var today = utcNow.Date;

        dateTimeServiceMock.Setup(s => s.GetUtcNow()).Returns(utcNow);
        dateTimeServiceMock.Setup(s => s.GetToday()).Returns(today);

        var scheduler = new DefaultRateRefreshScheduler(
            new TimeOnly(14, 30),
            TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"),
            dateTimeServiceMock.Object);

        var expectedRefreshTime = new DateTime(2025, 11, 10, 13, 30, 0);
        var result = scheduler.GetNextRefreshTime();

        Assert.Equal(expectedRefreshTime, result.DateTime);
    }
}
