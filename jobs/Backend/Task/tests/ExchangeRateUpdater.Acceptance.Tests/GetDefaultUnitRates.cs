using Adapter.ExchangeRateProvider.CzechNatBank.Tests.Integration;
using ExchangeRateUpdater.Acceptance.Tests.Dtos;
using FluentAssertions;
using Flurl;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ExchangeRateUpdater.Acceptance.Tests;

[TestFixture]
internal class GetDefaultUnitRates
{
    [Test, Ignore("Not working")]
    public async Task WhenCallingCzechNationalToGetDefaultUnitRates_ShouldReturnAListOfUnitExchangeRates()
    {
        // act
        var url = Global.Settings!.ExchangeRateUpdaterBaseAddress.AppendPathSegment("api/exchangeRates/defaultRates");
        using var client = CreateHttpClient();
        var response = await client.GetAsync(url);
        var content = JsonConvert.DeserializeObject<IEnumerable<ExchangeRateDto>>(await response.Content.ReadAsStringAsync());

        // assert
        content!.Count().Should().BeGreaterThan(0);
    }


    private HttpClient CreateHttpClient()
    {
        return new HttpClient();
    }
}
