using Adapter.Http.CNB.Repositories;
using Domain.Ports;
using FluentAssertions;
using NUnit.Framework;
using Serilog;

namespace Adapter.Http.CNB.Tests.Integration;

[TestFixture]
public class ExchangeRatesRepositoryTests
{
    [Test]
    public async Task GivenAValidDate_ShouldReturnListOfExchangeRates()
    {
        // arrange
        var sut = CreateSut();

        // act
        var exchangeRates = await sut.GetDailyExchangeRatesAsync(DateTime.Now, CancellationToken.None);

        // assert
        exchangeRates.Count.Should().BeGreaterThan(0);
    }

    [Test]
    public async Task GivenADate_WhenDateIsNoLongerValid_ShouldReturnAnEmptyList()
    {
        // arrange
        var sut = CreateSut();

        // act
        var exchangeRates = await sut.GetDailyExchangeRatesAsync(new DateTime(1923, 11, 25), CancellationToken.None);

        // assert
        exchangeRates.Count.Should().Be(0);
    }
    
    [Test]
    public async Task GivenADate_WhenDateIsInTheFuture_ShouldReturnSameListAsPresentDate()
    {
        // arrange
        var sut = CreateSut();

        // act
        var exchangeRatesPresent = await sut.GetDailyExchangeRatesAsync(DateTime.Now, CancellationToken.None);
        var exchangeRatesFuture = await sut.GetDailyExchangeRatesAsync(DateTime.Now.AddDays(1), CancellationToken.None);

        // assert
        exchangeRatesFuture.Should().BeEquivalentTo(exchangeRatesPresent);
    }

    private IExchangeRatesRepository CreateSut()
    {
        var loggerConfiguration = new LoggerConfiguration();
        var logger = loggerConfiguration.CreateLogger();
        
        var cnbSettings = new CNBSettings
        {
            BaseAddress = Global.Settings.BaseAddress
        };

        return new ExchangeRatesRepository(cnbSettings, logger);
    }
}