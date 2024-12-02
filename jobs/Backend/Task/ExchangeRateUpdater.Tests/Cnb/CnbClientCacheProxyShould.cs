using ExchangeRateUpdater.Cnb;
using W4k.Either;

namespace ExchangeRateUpdater.Tests.Cnb;

[Trait("Category", "Unit")]
public class CnbClientCacheProxyShould
{
    private static readonly CnbExchangeRate ExchangeRateEur = new()
    {
        ValidFor = DateTime.Today,
        CountryName = "EMU",
        CurrencyName = "euro",
        Amount = 1,
        CurrencyCode = "EUR",
        ExchangeRate = 24.415m,
    };

    private static readonly CnbExchangeRatesDto ExchangeRatesPayload = new()
    {
        Rates = new List<CnbExchangeRate>
        {
            ExchangeRateEur
        }
    };

    [Fact]
    public async Task NotCallClientTwice()
    {
        // arrange
        var client = new TestCnbClient(
            callCount =>
            {
                Assert.Equal(1, callCount);
                return ExchangeRatesPayload;
            });

        var proxy = new CnbClientCacheProxy(client, TimeSpan.FromMinutes(5));

        // act & assert
        _ = await proxy.GetExchangeRates(CancellationToken.None);
        _ = await proxy.GetExchangeRates(CancellationToken.None);
    }

    [Fact]
    public async Task CacheValueFromClient()
    {
        // arrange
        var client = new TestCnbClient(_ => ExchangeRatesPayload);
        var proxy = new CnbClientCacheProxy(client, TimeSpan.FromMinutes(5));

        // act
        _ = await proxy.GetExchangeRates(CancellationToken.None);
        var secondCallResult = await proxy.GetExchangeRates(CancellationToken.None);

        // assert
        Assert.True(secondCallResult.TryPick(out CnbExchangeRatesDto? exchangeRates));
        Assert.Equal(ExchangeRatesPayload, exchangeRates);
    }

    [Fact]
    public async Task NotCacheFailedResult()
    {
        // arrange
        var client = new TestCnbClient(
            callCount =>
            {
                if (callCount == 1)
                {
                    Assert.Equal(1, callCount);
                    return new CnbError();
                }

                Assert.Equal(2, callCount);
                return ExchangeRatesPayload;
            });

        var proxy = new CnbClientCacheProxy(client, TimeSpan.FromMinutes(5));

        // act & assert
        _ = await proxy.GetExchangeRates(CancellationToken.None);
        _ = await proxy.GetExchangeRates(CancellationToken.None);
        _ = await proxy.GetExchangeRates(CancellationToken.None);
    }

    // 💡 instead of using Moq, NSubstitute or what not, this simple test double will do just fine ¯\_(ツ)_/¯
    private sealed class TestCnbClient(Func<int, Either<CnbExchangeRatesDto, CnbError>> getCallback) : ICnbClient
    {
        private int _callCount;

        public Task<Either<CnbExchangeRatesDto, CnbError>> GetCurrentExchangeRates(CancellationToken cancellationToken)
        {
            var callCount = Interlocked.Increment(ref _callCount);
            var result = getCallback(callCount);

            return Task.FromResult(result);
        }
    }
}