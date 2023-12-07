using Adapter.ExchangeRateProvider.InMemory;
using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.ValueObjects;
using ExchangeRateUpdater.Host.WebApi.Tests.Unit.Dtos;
using FluentAssertions;
using FluentAssertions.Extensions;
using Flurl;
using Newtonsoft.Json;
using NUnit.Framework;
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
        key.ExpiryDate.Should().BeCloseTo(ReferenceTime.GetTime().Add(TodayTtl), 1.Seconds());
        key.LastAccessedTime.Should().BeAfter(creationTime);
    }


    [TestCase(11)]
    [TestCase(12)]
    [TestCase(60)]
    public async Task GivenAnAlreadyExpiredCachedKey_ShouldDeleteCacheValueAndMakeCallToProvider(int seconds)
    {
        // arrange
        ExchangeRateProviderRepository!.UpsertExchangeRate(ReferenceTime.GetTime(), new HashSet<ExchangeRate>
        {
            new ExchangeRate(new Currency("EUR"), new Currency("USD"), new PositiveRealNumber(0.92m))
        });
        var creationTime = DateTime.Now.AddSeconds(-seconds);
        ReferenceTime.SetTime(creationTime);

        // act
        var relativeUrl = "api".AppendPathSegment("exchangeRates").AppendPathSegment("defaultRates").SetQueryParam("date", creationTime);
        _ = await HttpClient!.GetAsync(relativeUrl);
        var cachedData = ExchangeRateCacheRepository!.GetCachedData();
        ReferenceTime.SetTime(DateTime.Now);
        var response = await HttpClient!.GetAsync(relativeUrl.SetQueryParam("date", DateTime.Now));

        
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var allFxRates = JsonConvert.DeserializeObject<IEnumerable<ExchangeRateDto>>(await response.Content.ReadAsStringAsync());
        allFxRates.Should().BeEquivalentTo(new List<ExchangeRateDto>
        {
            new ExchangeRateDto
            {
                From = "EUR",
                To   = "USD",
                ExchangeRate = 0.92m
            }
        });
        cachedData = ExchangeRateCacheRepository!.GetCachedData();
        cachedData.Should().NotBeNull();
        var cacheKey = new CacheKey(ReferenceTime.GetTime(), ReferenceTime.GetTime(), CacheType.All, creationTime, TodayTtl);

        cachedData.Keys.Should().NotContain(cacheKey);
    }

    [Test]
    public async Task WhenCacheSizeIsMax_ShouldEvictUsingLRU()
    {
        // arrange

        var notExpected = new HashSet<ExchangeRate>
        {
            new ExchangeRate(new Currency("CZK"), new Currency("USD"), new PositiveRealNumber(12.78m))
        };
        ExchangeRateProviderRepository!.UpsertExchangeRate(ReferenceTime.GetTime().AddDays(-2), notExpected);
        var expected1 = new HashSet<ExchangeRate>
        {
            new ExchangeRate(new Currency("MDL"), new Currency("USD"), new PositiveRealNumber(16.78m))
        };
        ExchangeRateProviderRepository!.UpsertExchangeRate(ReferenceTime.GetTime().AddDays(-1), expected1);

        var expected2 = new HashSet<ExchangeRate>
        {
            new ExchangeRate(new Currency("CZK"), new Currency("USD"), new PositiveRealNumber(16.78m))
        };
        ExchangeRateProviderRepository!.UpsertExchangeRate(ReferenceTime.GetTime(), expected2);
        var relativeUrl = "api".AppendPathSegment("exchangeRates").AppendPathSegment("defaultRates");

        _ = await HttpClient!.GetAsync(relativeUrl.SetQueryParam("requestDate", ReferenceTime.GetTime().AddDays(-2)));
        ReferenceTime.SetTime(ReferenceTime.GetTime().AddMinutes(1));
        _ = await HttpClient!.GetAsync(relativeUrl.SetQueryParam("requestDate", ReferenceTime.GetTime().AddDays(-1)));
        ReferenceTime.SetTime(ReferenceTime.GetTime().AddMinutes(2));
        var cachedData = ExchangeRateCacheRepository!.GetCachedData();
        cachedData.Count.Should().Be(2);

        // act
        ReferenceTime.SetTime(ReferenceTime.GetTime().AddMinutes(3));
        var response = await HttpClient!.GetAsync(relativeUrl.SetQueryParam("requestDate", ReferenceTime.GetTime()));

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var allFxRates = JsonConvert.DeserializeObject<IEnumerable<ExchangeRateDto>>(await response.Content.ReadAsStringAsync());
        allFxRates.Should().BeEquivalentTo(new List<ExchangeRateDto>
        {
            new ExchangeRateDto
            {
                From = "CZK",
                To   = "USD",
                ExchangeRate = 16.78m
            }
        });
        cachedData = ExchangeRateCacheRepository!.GetCachedData();
        cachedData.Should().NotBeNull();
        cachedData.Count.Should().Be(2);
        
        cachedData.Values.Select(cacheValue => cacheValue.Value).Should().BeEquivalentTo(new List<IEnumerable<ExchangeRate>> 
        { 
            expected1,
            expected2
        });
    }
}
