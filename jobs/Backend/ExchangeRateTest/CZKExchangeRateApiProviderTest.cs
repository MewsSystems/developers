using ExchangeRateUpdater;
using Moq;
using Moq.Protected;

namespace ExchangeRateTest;

public class CZKExchangeRateApiProviderTest
{
    private readonly Mock<CZKExchangeRateApiProvider> _mockApiProvider;
    private readonly ExchangeRateCNBResult eur_rate;
    private readonly ExchangeRateCNBResult jpy_rate;

    public CZKExchangeRateApiProviderTest()
    {
        eur_rate = new ExchangeRateCNBResult
        {
            validFor = DateTime.Now,
            country = "EMU",
            currency = "euro",
            amount = 1,
            currencyCode = "EUR",
            rate = (decimal)24.952
        };
        jpy_rate = new ExchangeRateCNBResult
        {
            validFor = DateTime.Now,
            country = "Japan",
            currency = "yen",
            amount = 100,
            currencyCode = "JPY",
            rate = (decimal)10.1
        };

        _mockApiProvider = new Mock<CZKExchangeRateApiProvider> { CallBase = true };
        _mockApiProvider.Protected().Setup<Task<ExchangeRateCNBApiResponse>>("PerformApiRequest").ReturnsAsync(
            new ExchangeRateCNBApiResponse
            {
                rates = new[] { eur_rate, jpy_rate }
            }
        );
    }

    [Fact]
    public async void WhenFetchesTheDataReturnsAResultsDictionary()
    {
        var exchangeRateProvider = _mockApiProvider.Object;

        var result = await exchangeRateProvider.FetchRates();

        Assert.Equal(2, result.ExchangeRates.Count);
        Assert.Equal(eur_rate.rate, result.GetRate(eur_rate.currencyCode, "CZK"));
        Assert.Equal(jpy_rate.rate/100, result.GetRate(jpy_rate.currencyCode, "CZK"));
    }

}
