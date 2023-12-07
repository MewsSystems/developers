using Adapter.ExchangeRateProvider.CzechNatBank.Tests.Integration;
using ExchangeRateUpdater.Acceptance.Tests.Dtos;
using FluentAssertions;
using Flurl;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ExchangeRateUpdater.Acceptance.Tests;

[TestFixture, Ignore("Not working")]
internal class ExchangeOrdersTests
{
    [Test]
    public async Task WhenCallingCzechNationalToExchange_ShouldReturnAExchangeResult()
    {
        // arrange
        var dto = new ExchangeOrderDto
        {
            SourceCurrency = "USD",
            TargetCurrency = "CZK",
            SumToExchange = 10
        };

        var contentToSend = new StringContent(JsonConvert.SerializeObject(dto), System.Text.Encoding.UTF8, "application/json");

        // act
        var url = Global.Settings!.ExchangeRateUpdaterBaseAddress.AppendPathSegment("api/exchangeRates/exchange");
        using var client = CreateHttpClient();
        var response = await client.PostAsync(url, contentToSend);
        var content = JsonConvert.DeserializeObject<ExchangeResultDto>(await response.Content.ReadAsStringAsync());

        // assert
        content!.SourceCurrency.Should().Be("USD");
        content.TargetCurrency.Should().Be("CZK");
        content.ConvertedSum.Should().BeGreaterThan(0);

    }


    private HttpClient CreateHttpClient()
    {
        return new HttpClient();
    }
}
