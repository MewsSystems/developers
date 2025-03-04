using System.Net;
using System.Net.Http.Json;
using ExchangeRateUpdater.Infrastructure.Cache;
using ExchangeRateUpdater.Infrastructure.CNB;
using ExchangeRateUpdater.Infrastructure.CNB.Entities;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;

namespace ExchangeRateUpdater.UnitTests.Infrastructure.CNB;

public class TestCnbProvider
{
    private class TestState
    {
        public Mock<ICacheProvider> CacheProvider { get; }
        public Mock<IOptions<CnbConfiguration>> CnbConfiguration { get; }
        public HttpClient CnbHttpClient { get; }

        public Mock<HttpMessageHandler> HttpMessageHandler { get; }
        public CnbProvider Subject { get; }

        public TestState()
        {
            CnbConfiguration = new Mock<IOptions<CnbConfiguration>>();
            CnbConfiguration.Setup(a => a.Value)
                .Returns(new CnbConfiguration()
                {
                    BaseAddress = "https://fakeAddress.com",
                    CacheTTLInSeconds = 10,
                    CacheKeyBase = "key-base",
                    Language = "EN"
                });
            CacheProvider = new Mock<ICacheProvider>();
            HttpMessageHandler = new Mock<HttpMessageHandler>();
            CnbHttpClient = new HttpClient(HttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://fakeAddress.com")
            };


            Subject = new CnbProvider(CacheProvider.Object, CnbHttpClient, CnbConfiguration.Object);
        }

        public CnbExchangeRateEntity GetDefaultRate(string code, decimal rate)
        {
            return new CnbExchangeRateEntity()
            {
                Amount = 1,
                Country = "fakeCountry",
                Currency = "fakeCurrency",
                Order = 1,
                CurrencyCode = code,
                ValidFor = DateOnly.FromDateTime(DateTime.UtcNow),
                Rate = rate
            };
        }
    }

    [Fact]
    public async Task WhenRatesAreInCache_ThenReturnFromCache()
    {
        TestState state = new TestState();
        CnbExchangeRateEntity rate1 = state.GetDefaultRate("AAA", 123.41m);
        CnbExchangeRateEntity rate2 = state.GetDefaultRate("BBB", 111.22m);
        CnbExchangeResponseEntity responseEntity = new CnbExchangeResponseEntity()
        {
            Rates = [rate1, rate2]
        };
        state.CacheProvider.Setup(a => a.TryGetCache(It.IsAny<string>(), out responseEntity))
            .Returns(true);

        CnbExchangeResponseEntity result = await state.Subject.GetLatestExchangeInformation();

        Assert.Equal(2, result.Rates.Length);
        Assert.Equal(rate1.CurrencyCode, result.Rates[0].CurrencyCode);
        Assert.Equal(rate2.CurrencyCode, result.Rates[1].CurrencyCode);

        state.CacheProvider.Verify(a =>
                a.TryGetCache(state.CnbConfiguration.Object.Value.CacheKeyBase,
                    out responseEntity),
            Times.Once);
        state.CacheProvider.Verify(a => a.TrySetCache(It.IsAny<string>(),
            It.IsAny<CnbExchangeResponseEntity>(), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task WhenRatesAreNotInCache_Query3rdPartyDependency_ThenStoreInCache()
    {
        TestState state = new TestState();
        CnbExchangeResponseEntity? nullExchangeResponseEntity = null;

        state.CacheProvider.Setup(a => a.TryGetCache(It.IsAny<string>(), out nullExchangeResponseEntity))
            .Returns(false);

        CnbExchangeRateEntity rate1 = state.GetDefaultRate("AAA", 123.41m);
        CnbExchangeRateEntity rate2 = state.GetDefaultRate("BBB", 111.22m);
        CnbExchangeResponseEntity responseEntity = new CnbExchangeResponseEntity()
        {
            Rates = [rate1, rate2]
        };

        state.CacheProvider.Setup(a => a.TrySetCache(It.IsAny<string>(), responseEntity, It.IsAny<int>()))
            .Returns(true);

        state.HttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(responseEntity)
            });

        CnbExchangeResponseEntity result = await state.Subject.GetLatestExchangeInformation();

        Assert.Equal(2, result.Rates.Length);
        Assert.Equal(rate1.CurrencyCode, result.Rates[0].CurrencyCode);
        Assert.Equal(rate2.CurrencyCode, result.Rates[1].CurrencyCode);

        state.CacheProvider.Verify(a => a.TryGetCache(It.IsAny<string>(), out nullExchangeResponseEntity), Times.Once);
        state.CacheProvider.Verify(a =>
                a.TrySetCache(state.CnbConfiguration.Object.Value.CacheKeyBase,
                    It.IsAny<CnbExchangeResponseEntity>(), state.CnbConfiguration.Object.Value.CacheTTLInSeconds),
            Times.Once);
    }
}