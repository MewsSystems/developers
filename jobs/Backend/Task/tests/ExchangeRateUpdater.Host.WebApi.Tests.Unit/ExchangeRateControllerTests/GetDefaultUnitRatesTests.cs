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
internal class GetDefaultUnitRatesTests : ControllerTestBase
{
    

    [Test]
    public async Task GivenNoDefaultUnitRatesStored_WhenQueryingGetDefaultUnitRates_ShouldReturnEmptyList()
    {
        // act
        var relativeUrl = "api".AppendPathSegment("exchangeRates").AppendPathSegment("default");
        var response = await HttpClient.GetAsync(relativeUrl);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var defaultExhangeRates = JsonConvert.DeserializeObject<IEnumerable<ExchangeRateDto>>(await response.Content.ReadAsStringAsync());
        defaultExhangeRates.Should().BeEmpty();
    }

    [Test]
    public async Task GivenSeveralUnitRatesStored_WhenQueryingGetDefaultUnitRates_ShouldTheExchangeRates()
    {
        // arrange
        ExchangeRateProviderRepository.UpsertExchangeRate(new ExchangeRate(new Currency("MDL"), new Currency("USD"), new PositiveRealNumber(17.78m)));

        // act
        var relativeUrl = "api".AppendPathSegment("exchangeRates").AppendPathSegment("default");
        var response = await HttpClient.GetAsync(relativeUrl);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var defaultExhangeRates = JsonConvert.DeserializeObject<IEnumerable<ExchangeRateDto>>(await response.Content.ReadAsStringAsync());
        defaultExhangeRates.Should().BeEquivalentTo(new List<ExchangeRateDto>
        {
            new ExchangeRateDto
            {
                From = "MDL",
                To   = "USD",
                ExchangeRate = 17.78m
            }
        });
    }
}
