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
internal class GetAllFxRatesAsyncTests : ControllerTestBase
{
    [Test]
    public async Task GivenNoFXRatesStored_WhenQueryingGetFxRates_ShouldReturnEmptyList()
    {
        // act
        var relativeUrl = "api".AppendPathSegment("exchangeRates").AppendPathSegment("defaultRates");
        var response = await HttpClient!.GetAsync(relativeUrl);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var defaultExhangeRates = JsonConvert.DeserializeObject<IEnumerable<ExchangeRateDto>>(await response.Content.ReadAsStringAsync());
        defaultExhangeRates.Should().BeEmpty();
    }

    [Test]
    public async Task GivenSeveralRatesStored_WhenQueryingGetRates_ShouldReturnTheExchangeRates()
    {
        // arrange
        var referenceTime = DateTime.Now;
        ExchangeRateProviderRepository!.UpsertExchangeRate(referenceTime, new HashSet<ExchangeRate>
        {
            new ExchangeRate(new Currency("MDL"), new Currency("USD"), new PositiveRealNumber(17.78m), referenceTime),
            new ExchangeRate(new Currency("EUR"), new Currency("USD"), new PositiveRealNumber(0.92m), referenceTime)
        }) ;

        // act
        var relativeUrl = "api".AppendPathSegment("exchangeRates").AppendPathSegment("defaultRates");
        var response = await HttpClient!.GetAsync(relativeUrl);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var defaultExhangeRates = JsonConvert.DeserializeObject<IEnumerable<ExchangeRateDto>>(await response.Content.ReadAsStringAsync());
        defaultExhangeRates.Should().BeEquivalentTo(new List<ExchangeRateDto>
        {
            new ExchangeRateDto
            {
                From = "MDL",
                To   = "USD",
                ExchangeRate = 17.78m,
                ExchangeRateTime = referenceTime,
            },
            new ExchangeRateDto
            {
                From = "EUR",
                To   = "USD",
                ExchangeRate = 0.92m,
                ExchangeRateTime = referenceTime,
            }
        });
    }

    [Test]
    public async Task GivenSeveralRatesForDifferentDatesStored_WhenQueryingGetRatesWithDate_ShouldReturnTheExchangeRateBeforeOrEqualToRequestedDate()
    {
        // arrange
        var referenceTime = DateTime.Now;
        ExchangeRateProviderRepository!.UpsertExchangeRate(DateTime.Now, new HashSet<ExchangeRate>
        {
            new ExchangeRate(new Currency("MDL"), new Currency("USD"), new PositiveRealNumber(17.78m), referenceTime)
        });
        // arrange
        ExchangeRateProviderRepository.UpsertExchangeRate(DateTime.Now.AddDays(-2), new HashSet<ExchangeRate>
        {
            new ExchangeRate(new Currency("MDL"), new Currency("USD"), new PositiveRealNumber(16.78m), referenceTime)
        });

        // act
        var relativeUrl = "api".AppendPathSegment("exchangeRates").AppendPathSegment("defaultRates").SetQueryParam("requestDate", DateTime.Now.AddDays(-1));
        var response = await HttpClient!.GetAsync(relativeUrl);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var defaultExhangeRates = JsonConvert.DeserializeObject<IEnumerable<ExchangeRateDto>>(await response.Content.ReadAsStringAsync());
        defaultExhangeRates.Should().BeEquivalentTo(new List<ExchangeRateDto>
        {
            new ExchangeRateDto
            {
                From = "MDL",
                To   = "USD",
                ExchangeRate = 16.78m,
                ExchangeRateTime = referenceTime
            }
        });
    }
}
