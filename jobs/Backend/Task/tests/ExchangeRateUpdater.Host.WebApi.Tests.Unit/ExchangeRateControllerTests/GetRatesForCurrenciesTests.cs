using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.ValueObjects;
using ExchangeRateUpdater.Host.WebApi.Tests.Unit.Dtos;
using FluentAssertions;
using Flurl;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;

namespace ExchangeRateUpdater.Host.WebApi.Tests.Unit.ExchangeRateControllerTests;

[TestFixture]
internal class GetRatesForCurrenciesTests : ControllerTestBase
{
    [Test]
    public async Task GivenNoFXRatesStored_WhenQueryingGetRatesForCurrencies_ShouldReturnEmptyList()
    {
        // act
        var currencies = new List<string> { "EUR", "USD" };
        var content = new StringContent(JsonConvert.SerializeObject(currencies), System.Text.Encoding.UTF8, "application/json");

        var relativeUrl = "api".AppendPathSegment("exchangeRates").AppendPathSegment("getRatesForCurrencies").SetQueryParam("requestDate", DateTime.Now);
        var response = await HttpClient!.PostAsync(relativeUrl, content);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var defaultExhangeRates = JsonConvert.DeserializeObject<IEnumerable<ExchangeRateDto>>(await response.Content.ReadAsStringAsync());
        defaultExhangeRates.Should().BeEmpty();
    }

    [Test]
    public async Task GivenMultipleFXRatesStored_WhenQueryingGetRatesForCurrencies_ShouldReturnEmptyList()
    {
        // arrange
        var referenceTime = DateTime.Now;
        ExchangeRateProviderRepository!.UpsertExchangeRate(referenceTime, new HashSet<ExchangeRate>
        {
            new ExchangeRate(new Currency("USD"), new Currency("CZK"), new PositiveRealNumber(22.55m), referenceTime),
            new ExchangeRate(new Currency("EUR"), new Currency("CZK"), new PositiveRealNumber(24.29m), referenceTime)
        });

        // act
        var currencies = new List<string> { "EUR" };
        var content = new StringContent(JsonConvert.SerializeObject(currencies), System.Text.Encoding.UTF8, "application/json");

        var relativeUrl = "api".AppendPathSegment("exchangeRates").AppendPathSegment("getRatesForCurrencies").SetQueryParam("requestDate", DateTime.Now);
        var response = await HttpClient!.PostAsync(relativeUrl, content);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var defaultExhangeRates = JsonConvert.DeserializeObject<IEnumerable<ExchangeRateDto>>(await response.Content.ReadAsStringAsync());
        defaultExhangeRates.Should().BeEquivalentTo(new List<ExchangeRateDto>
        {
            new ExchangeRateDto
            {
                From = "EUR",
                To = "CZK",
                ExchangeRate = 24.29m,
                ExchangeRateDate = referenceTime
            }
        });
    }
}
