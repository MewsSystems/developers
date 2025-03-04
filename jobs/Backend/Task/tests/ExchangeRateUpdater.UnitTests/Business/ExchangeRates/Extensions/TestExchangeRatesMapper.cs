using ExchangeRateUpdater.Business.ExchangeRates;
using ExchangeRateUpdater.Business.ExchangeRates.Extensions;
using ExchangeRateUpdater.Dto.ExchangeRates;
using ExchangeRateUpdater.Infrastructure.CNB.Entities;

namespace ExchangeRateUpdater.UnitTests.Business.ExchangeRates.Extensions;

public class TestExchangeRatesMapper
{
    [Fact]
    public void ToExchangeRate_MapsCnbExchangeRateEntityToExchangeRate()
    {
        CnbExchangeRateEntity cnbExchangeRateEntity = new CnbExchangeRateEntity()
        {
            CurrencyCode = "USD",
            Rate = 1.23m,
            Amount = 1,
            Country = "USA",
            Currency = "Dollar",
            Order = 1,
            ValidFor = DateOnly.FromDateTime(DateTime.UtcNow)
        };

        ExchangeRate result = cnbExchangeRateEntity.ToExchangeRate();

        Assert.NotNull(result);
        Assert.Equal(ExchangeRateConstants.SourceCurrencyExchangeRate, result.SourceCurrency.Code);
        Assert.Equal(cnbExchangeRateEntity.CurrencyCode, result.TargetCurrency.Code);
        Assert.Equal(cnbExchangeRateEntity.Rate, result.Value);
        Assert.Equal($"{ExchangeRateConstants.SourceCurrencyExchangeRate}/{cnbExchangeRateEntity.CurrencyCode}={cnbExchangeRateEntity.Rate}", result.ToString());
    }
}