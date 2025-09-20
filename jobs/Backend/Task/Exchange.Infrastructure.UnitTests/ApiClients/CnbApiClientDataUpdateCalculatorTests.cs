using Exchange.Infrastructure.ApiClients;
using Exchange.Infrastructure.DateTimeProviders;
using FluentAssertions;
using Moq;

namespace Exchange.Infrastructure.UnitTests.ApiClients;

public class CnbApiClientDataUpdateCalculatorTests
{
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
    private readonly CnbApiClientDataUpdateCalculator _calculator;

    public CnbApiClientDataUpdateCalculatorTests()
    {
        _dateTimeProviderMock = new Mock<IDateTimeProvider>();
        _calculator = new CnbApiClientDataUpdateCalculator(_dateTimeProviderMock.Object);
    }

    [Fact]
    public void GetNextExpectedUpdateDate_WhenCurrentTimeIsBeforeUpdateTimeOnWorkingDay_ShouldReturnSameDay()
    {
        // Arrange
        var currentDateWednesday = new DateOnly(2025, 10, 15);
        var timeBeforeDataUpdateTime = new TimeOnly(10, 0);
        var currentDateTime = currentDateWednesday.ToDateTime(timeBeforeDataUpdateTime);

        _dateTimeProviderMock.Setup(p => p.Now).Returns(currentDateTime);

        // Act
        var result = _calculator.GetNextExpectedUpdateDate(currentDateWednesday);

        // Assert
        result.Should().Be(currentDateWednesday.ToDateTime(new TimeOnly(14, 30)));
    }

    [Fact]
    public void GetNextExpectedUpdateDate_WhenCurrentTimeIsAfterUpdateTimeOnWorkingDay_ShouldReturnNextWorkingDay()
    {
        // Arrange
        var currentDateWednesday = new DateOnly(2025, 10, 15);
        var timeAfterDataUpdateTime = new TimeOnly(15, 0);
        var currentDateTime = currentDateWednesday.ToDateTime(timeAfterDataUpdateTime);

        _dateTimeProviderMock.Setup(p => p.Now).Returns(currentDateTime);

        // Act
        var result = _calculator.GetNextExpectedUpdateDate(currentDateWednesday);

        // Assert
        var expectedDate = new DateOnly(2025, 10, 16);
        result.Should().Be(expectedDate.ToDateTime(new TimeOnly(14, 30)));
    }

    [Fact]
    public void GetNextExpectedUpdateDate_WhenCurrentDateIsWeekend_ShouldReturnNextWorkingDay()
    {
        // Arrange
        var currentDateSaturday = new DateOnly(2025, 10, 18);
        var currentDateTime = currentDateSaturday.ToDateTime(new TimeOnly(10, 0));

        _dateTimeProviderMock.Setup(p => p.Now).Returns(currentDateTime);

        // Act
        var result = _calculator.GetNextExpectedUpdateDate(currentDateSaturday);

        // Assert
        var expectedDate = new DateOnly(2025, 10, 20);
        result.Should().Be(expectedDate.ToDateTime(new TimeOnly(14, 30)));
    }

    [Fact]
    public void GetNextExpectedUpdateDate_WhenCurrentDateIsHoliday_ShouldReturnNextWorkingDay()
    {
        // Arrange
        var holidayDate = new DateOnly(2025, 12, 25);
        var currentDateTime = holidayDate.ToDateTime(new TimeOnly(10, 0));

        _dateTimeProviderMock.Setup(p => p.Now).Returns(currentDateTime);

        // Act
        var result = _calculator.GetNextExpectedUpdateDate(holidayDate);

        // Assert
        var expectedDate = new DateOnly(2025, 12, 26);
        result.Should().Be(expectedDate.ToDateTime(new TimeOnly(14, 30)));
    }

    [Fact]
    public void GetNextExpectedUpdateDate_WhenNextDayIsWeekend_ShouldSkipWeekendDays()
    {
        // Arrange
        var currentDateFriday = new DateOnly(2025, 10, 17);
        var timeAfterDataUpdateTime = currentDateFriday.ToDateTime(new TimeOnly(15, 0));

        _dateTimeProviderMock.Setup(p => p.Now).Returns(timeAfterDataUpdateTime);

        // Act
        var result = _calculator.GetNextExpectedUpdateDate(currentDateFriday);

        // Assert
        var expectedDate = new DateOnly(2025, 10, 20);
        result.Should().Be(expectedDate.ToDateTime(new TimeOnly(14, 30)));
    }

    [Fact]
    public void GetNextExpectedUpdateDate_WhenNextDayIsHoliday_ShouldSkipHoliday()
    {
        // Arrange
        var dayBeforeHoliday = new DateOnly(2025, 11, 30);
        var currentDateTime = dayBeforeHoliday.ToDateTime(new TimeOnly(15, 0));

        _dateTimeProviderMock.Setup(p => p.Now).Returns(currentDateTime);

        // Act
        var result = _calculator.GetNextExpectedUpdateDate(dayBeforeHoliday);

        // Assert
        var expectedDate = new DateOnly(2025, 12, 1);
        result.Should().Be(expectedDate.ToDateTime(new TimeOnly(14, 30)));
    }

    [Fact]
    public void GetNextExpectedUpdateDate_WhenMultipleNonWorkingDaysAhead_ShouldFindFirstWorkingDay()
    {
        // Arrange
        var dayBeforeWeekendAndHolidays = new DateOnly(2025, 12, 5);
        var currentDateTime = dayBeforeWeekendAndHolidays.ToDateTime(new TimeOnly(15, 0));

        _dateTimeProviderMock.Setup(p => p.Now).Returns(currentDateTime);

        // Act
        var result = _calculator.GetNextExpectedUpdateDate(dayBeforeWeekendAndHolidays);

        // Assert
        var expectedDate = new DateOnly(2025, 12, 9);
        result.Should().Be(expectedDate.ToDateTime(new TimeOnly(14, 30)));
    }
}