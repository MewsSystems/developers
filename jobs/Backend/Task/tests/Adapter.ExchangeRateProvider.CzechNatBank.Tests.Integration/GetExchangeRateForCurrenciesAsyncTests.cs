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
        var result = await sut.GetExchangeRateForCurrenciesAsync(new Currency("USD"), new Currency("CZK"), new DateTime(2023, 1, 1), new DateTime(2023, 1, 2));
        result.ToList().Count.Should().BeGreaterThan(0);
    }


    [Test]
    public void GivenInvalidTargetCurrency_WhenCallingCzechNationalBankToGetExchange_ShouldThrow()
    {
        // act
        var sut = CreateSut();

        // assert
        var exception = Assert.ThrowsAsync<NotSupportedException>(async () => await sut.GetExchangeRateForCurrenciesAsync(new Currency("CZK"), new Currency("USD"), new DateTime(2023, 1, 1), new DateTime(2023, 1, 2)));
        exception!.Message.Should().Be("Target currencies besides CZK are not yet supported.");
    }

    
}
