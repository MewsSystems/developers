using ExchangeRateUpdater.Domain.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace Adapter.ExchangeRateProvider.CzechNatBank.Tests.Integration;

[TestFixture]
internal class GetExchangeRateForCurrenciesAsyncTests : TestBase
{
    

    [Test]
    public async Task GivenValidExchangeOrder_WhenCallingCzechNationalBankToGetExchange_ShouldReturnAListOfUnitExchangeRatesSortedByDate()
    {
        // act
        var sut = CreateSut();

        // assert
        var result = await sut.GetExchangeRateForCurrenciesAsync(new Currency("USD"), new Currency("CZK"), 
                                                                new DateTime(2023, 1, 1), new DateTime(2023, 1, 2), CancellationToken.None);
        result.ToList().Count.Should().BeGreaterThan(0);
    }


    [Test]
    public void GivenInvalidTargetCurrency_WhenCallingCzechNationalBankToGetExchange_ShouldThrowNotSupportedException()
    {
        // act
        var sut = CreateSut();

        // assert
        var exception = Assert.ThrowsAsync<NotSupportedException>(async () => 
        await sut.GetExchangeRateForCurrenciesAsync(new Currency("CZK"), new Currency("USD"), 
                                                    new DateTime(2023, 1, 1), new DateTime(2023, 1, 2), CancellationToken.None));
        exception!.Message.Should().Be("Target currencies besides CZK are not yet supported.");
    }

    [Test]
    public async Task GivenAHolidayInterval_WhenCallingCzechNationalBankToGetExchange_ShouldReturnEmptyList()
    {
        // act
        var sut = CreateSut();


        var result = await sut.GetExchangeRateForCurrenciesAsync(new Currency("USD"), new Currency("CZK"),
                                                    // These are holiday days
                                                    new DateTime(2023, 12, 2), new DateTime(2023, 12, 3), CancellationToken.None);

        // assert
        result.Should().BeEmpty();
    }
}
