using ExchangeRateUpdater;

namespace ExchangeRateUpdaterTests;

public class CzkExchangeRateFactoryTests
{
    private readonly CzkExchangeRateFactory _factory;

    public CzkExchangeRateFactoryTests()
    {
        _factory = new CzkExchangeRateFactory();
    }

    [Fact]
    public void Create_WithExchangeRateResponse_FieldsAreMapped()
    {
        const string TargetCurrencyCode = "AUD";
        const decimal Rate = 29;

        var rate = _factory.Create(new ExchangeRateProvider.ExchangeRateResponse(TargetCurrencyCode, Rate, 1));

        Assert.Same(ExchangeRateProvider.CZECH_KORUNA_CODE, rate.SourceCurrency.Code);
        Assert.Same(TargetCurrencyCode, rate.TargetCurrency.Code);
        Assert.Equal(Rate, rate.Value);
    }

    [Theory]
    [InlineData(29, 1, 29)]
    [InlineData(28.113, 100, 0.28113)]
    [InlineData(1.442, 1000, 0.001442)]
    public void Create_WhenRateIncludesAmount_ValueIsAsExpected(decimal rate, int amount, decimal expected)
    {
        var exchangeRate = _factory.Create(new ExchangeRateProvider.ExchangeRateResponse("TEST", rate, amount));

        Assert.Equal(expected, exchangeRate.Value);
    }
}


