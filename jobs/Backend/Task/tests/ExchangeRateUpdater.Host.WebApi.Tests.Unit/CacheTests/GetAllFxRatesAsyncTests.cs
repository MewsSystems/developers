using Adapter.ExchangeRateProvider.InMemory;
using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Ports;
using ExchangeRateUpdater.Domain.ValueObjects;
using ExchangeRateUpdater.Host.WebApi.Tests.Unit.Dtos;
using FluentAssertions;
using FluentAssertions.Extensions;
using Flurl;
using Newtonsoft.Json;
using NUnit.Framework;
using Serilog.Sinks.InMemory;
using System.Net;

namespace ExchangeRateUpdater.Host.WebApi.Tests.Unit.CacheTests;

[TestFixture]
internal class GetAllFxRatesAsyncTests : ControllerCacheTestBase
{
    [Test]
    public async Task WhenFirstTimeGetAllFxRatesAsync_ShouldStoreValueInCache()
    {
        // arrange
        ExchangeRateProviderRepository!.UpsertExchangeRate(DateTime.Now, new HashSet<ExchangeRate>
        {
            new ExchangeRate(new Currency("MDL"), new Currency("USD"), new PositiveRealNumber(17.78m)),
            new ExchangeRate(new Currency("EUR"), new Currency("USD"), new PositiveRealNumber(0.92m))
        });

        // act
        var relativeUrl = "api".AppendPathSegment("exchangeRates").AppendPathSegment("defaultRates");
        var response = await HttpClient!.GetAsync(relativeUrl);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var allFxRates = JsonConvert.DeserializeObject<IEnumerable<ExchangeRateDto>>(await response.Content.ReadAsStringAsync());
        allFxRates.Should().BeEquivalentTo(new List<ExchangeRateDto>
        {
            new ExchangeRateDto
            {
                From = "MDL",
                To   = "USD",
                ExchangeRate = 17.78m
            },
            new ExchangeRateDto
            {
                From = "EUR",
                To   = "USD",
                ExchangeRate = 0.92m
            }
        });
        var cachedData = ExchangeRateCacheRepository!.GetCachedData();
        cachedData.Should().NotBeNull();
        var cacheKey = new CacheKey(DateTime.Now.Date, DateTime.Now.Date, CacheType.All, DateTime.Now, TodayTtl);

        cachedData[cacheKey].Value.Should().BeEquivalentTo(new List<ExchangeRate>
        {
            new ExchangeRate
            (
             new Currency("MDL"),
             new Currency("USD"),
             new PositiveRealNumber(17.78m)
            ),
            new ExchangeRate
            (
             new Currency("EUR"),
             new Currency("USD"),
             new PositiveRealNumber(0.92m)
            )
        });
    }

    [Test]
    public async Task WhenMultipleTimesGetAllFxRatesAsync_ShouldReturnedCachedValue()
    {
        // arrange
        ExchangeRateProviderRepository!.UpsertExchangeRate(DateTime.Now, new HashSet<ExchangeRate>
        {
            new ExchangeRate(new Currency("MDL"), new Currency("USD"), new PositiveRealNumber(17.78m)),
            new ExchangeRate(new Currency("EUR"), new Currency("USD"), new PositiveRealNumber(0.92m))
        });
        var creationTime = DateTime.Now.AddSeconds(-3);

        // act
        var relativeUrl = "api".AppendPathSegment("exchangeRates").AppendPathSegment("defaultRates").SetQueryParam("date", creationTime);
        _ = await HttpClient!.GetAsync(relativeUrl);
        var response = await HttpClient!.GetAsync(relativeUrl);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var allFxRates = JsonConvert.DeserializeObject<IEnumerable<ExchangeRateDto>>(await response.Content.ReadAsStringAsync());
        allFxRates.Should().BeEquivalentTo(new List<ExchangeRateDto>
        {
            new ExchangeRateDto
            {
                From = "MDL",
                To   = "USD",
                ExchangeRate = 17.78m
            },
            new ExchangeRateDto
            {
                From = "EUR",
                To   = "USD",
                ExchangeRate = 0.92m
            }
        });
        var cachedData = ExchangeRateCacheRepository!.GetCachedData();
        cachedData.Should().NotBeNull();
        var cacheKey = new CacheKey(DateTime.Now.Date, DateTime.Now.Date, CacheType.All, creationTime, TodayTtl);

        cachedData[cacheKey].Value.Should().BeEquivalentTo(new List<ExchangeRate>
        {
            new ExchangeRate
            (
             new Currency("MDL"),
             new Currency("USD"),
             new PositiveRealNumber(17.78m)
            ),
            new ExchangeRate
            (
             new Currency("EUR"),
             new Currency("USD"),
             new PositiveRealNumber(0.92m)
            )
        });

        var key = cachedData.Keys.First(key => cacheKey.Equals(key));
        key.ExpiryDate.Should().BeCloseTo(DateTime.Now.Add(TodayTtl), 1.Seconds());
        key.LastAccessedTime.Should().BeAfter(creationTime);
    }

}
