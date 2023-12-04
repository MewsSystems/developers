using FluentAssertions;
using NUnit.Framework;

namespace Adapter.ExchangeRateProvider.CzechNatBank.Tests.Integration;

[TestFixture]
internal class GetDefaultUnitRates : TestBase
{
    [Test]
    public async Task WhenCallingCzechNationalToGetDefaultUnitRates_ShouldReturnAListOfUnitExchangeRates()
    {
        // act
        var sut = CreateSut();

        // assert
        var result = await sut.GetDefaultUnitRates(DateTime.Now);
        result.ToList().Count.Should().BeGreaterThan(0);
    }
}
