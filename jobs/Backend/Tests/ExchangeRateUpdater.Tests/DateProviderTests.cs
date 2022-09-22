using ExchangeRateUpdater.Interfaces;
using Moq;

namespace ExchangeRateUpdater.Tests;

public class DateProviderTests
{
    private readonly Mock<IDateTimeService> _dateTimeServiceMock;
    
    private readonly DateProvider _provider;

    public DateProviderTests()
    {
        _dateTimeServiceMock = new Mock<IDateTimeService>();
        _provider = new DateProvider(_dateTimeServiceMock.Object);
    }
    
    [Fact]
    public void ForToday_BeforeRelease_PreviousDay()
    {
        var before = new DateTime(2022, 09, 22, 13, 12, 00);
        _dateTimeServiceMock.SetupGet(g => g.UtcNow).Returns(before);
        
        var result = _provider.ForToday();

        var expected = DateOnly.Parse("2022-09-21");
        Assert.Equal(expected, result);
    }
    
    [Fact]
    public void ForToday_AfterRelease_CurrentDay()
    {
        var before = new DateTime(2022, 09, 22, 14, 31, 00);
        _dateTimeServiceMock.SetupGet(g => g.UtcNow).Returns(before);
        
        var result = _provider.ForToday();

        var expected = DateOnly.Parse("2022-09-22");
        Assert.Equal(expected, result);
    }
}