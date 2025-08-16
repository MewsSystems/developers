using Adapter.ExchangeRateProvider.InMemory;
using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.ValueObjects;
using ExchangeRateUpdater.Host.WebApi.Tests.Unit.Dtos;
using FluentAssertions;
using Flurl;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
namespace ExchangeRateUpdater.Host.WebApi.Tests.Unit.CacheTests;

[TestFixture]
internal class GetExchangeRateForCurrenciesAsyncTests : ControllerCacheTestBase
{
    [Test]
    public async Task WhenFirstTimeGetExchangeRateForCurrenciesAsync_ShouldStoreValueInCache()
    {
        // arrange
        var referenceTime = ReferenceTime.GetTime();
        var expected = new HashSet<ExchangeRate>
        {
            new ExchangeRate(new Currency("USD"), new Currency("CZK"), new PositiveRealNumber(22.55m), referenceTime)
        };

        ExchangeRateProviderRepository!.UpsertExchangeRate(referenceTime, expected);

        // act
        var exchangeOrderDto = new ExchangeOrderDto
        {
            SourceCurrency = "USD",
            TargetCurrency = "CZK",
            SumToExchange = 10
        };
        var stringContent = new StringContent(JsonConvert.SerializeObject(exchangeOrderDto), System.Text.Encoding.UTF8, "application/json");
        var relativeUrl = "api".AppendPathSegment("exchangeRates").AppendPathSegment("exchange").SetQueryParam("requestDate", ReferenceTime.GetTime());
        var response = await HttpClient!.PostAsync(relativeUrl, stringContent);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var exchangeResult = JsonConvert.DeserializeObject<ExchangeResultDto>(await response.Content.ReadAsStringAsync());
        exchangeResult.Should().BeEquivalentTo(new ExchangeResultDto
        {
            SourceCurrency = "USD",
            TargetCurrency = "CZK",
            ConvertedSum = 10 * 22.55m,
            ExchangeRateDate = referenceTime
        });
        var cachedData = ExchangeRateCacheRepository!.GetCachedData();
        cachedData.Should().NotBeNull();
        var cacheKey = new CacheKey(referenceTime.Date, referenceTime.Date, CacheType.Selected, referenceTime, TodayTtl);

        cachedData[cacheKey].Value.Should().BeEquivalentTo(expected);
    }
}
