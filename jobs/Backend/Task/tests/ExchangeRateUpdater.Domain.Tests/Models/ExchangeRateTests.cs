using ExchangeRateUpdater.Domain.Models;

namespace ExchangeRateUpdater.Domain.Tests.Models;

public class ExchangeRateTests
{
    [Fact]
    public void ExchangeRate_WithValidData_ShouldCreateSuccessfully()
    {
        var sourceCurrency = new Currency("USD");
        var targetCurrency = new Currency("EUR");
        var exchangeValue = 0.85m;
        var providerName = "TestProvider";
        var validUntil = DateTime.UtcNow.AddDays(1);

        var exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, exchangeValue, providerName, validUntil);

        Assert.Equal(sourceCurrency, exchangeRate.SourceCurrency);
        Assert.Equal(targetCurrency, exchangeRate.TargetCurrency);
        Assert.Equal(exchangeValue, exchangeRate.ExchangeValue);
        Assert.Equal(providerName, exchangeRate.ProviderName);
        Assert.Equal(validUntil, exchangeRate.ValidUntil);
    }

    [Fact]
    public void ExchangeRate_WithNullSourceCurrency_ShouldThrowArgumentNullException()
    {
        var targetCurrency = new Currency("EUR");
        var exchangeValue = 0.85m;

        Assert.Throws<ArgumentNullException>(() => new ExchangeRate(null!, targetCurrency, exchangeValue));
    }

    [Fact]
    public void ExchangeRate_WithNullTargetCurrency_ShouldThrowArgumentNullException()
    {
        var sourceCurrency = new Currency("USD");
        var exchangeValue = 0.85m;

        Assert.Throws<ArgumentNullException>(() => new ExchangeRate(sourceCurrency, null!, exchangeValue));
    }

    [Fact]
    public void ExchangeRate_WithNegativeExchangeValue_ShouldThrowArgumentException()
    {
        var sourceCurrency = new Currency("USD");
        var targetCurrency = new Currency("EUR");
        var exchangeValue = -0.85m;

        Assert.Throws<ArgumentException>(() => new ExchangeRate(sourceCurrency, targetCurrency, exchangeValue));
    }

    [Fact]
    public void ExchangeRate_WithZeroExchangeValue_ShouldThrowArgumentException()
    {
        var sourceCurrency = new Currency("USD");
        var targetCurrency = new Currency("EUR");
        var exchangeValue = 0m;

        Assert.Throws<ArgumentException>(() => new ExchangeRate(sourceCurrency, targetCurrency, exchangeValue));
    }

    [Fact]
    public void ExchangeRate_WithOptionalParameters_ShouldCreateSuccessfully()
    {
        var sourceCurrency = new Currency("USD");
        var targetCurrency = new Currency("EUR");
        var exchangeValue = 0.85m;

        var exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, exchangeValue);

        Assert.Equal(sourceCurrency, exchangeRate.SourceCurrency);
        Assert.Equal(targetCurrency, exchangeRate.TargetCurrency);
        Assert.Equal(exchangeValue, exchangeRate.ExchangeValue);
        Assert.Null(exchangeRate.ProviderName);
        Assert.Null(exchangeRate.ValidUntil);
    }

    [Fact]
    public void ExchangeRate_Equals_ShouldReturnTrue_WhenSameProperties()
    {
        var sourceCurrency = new Currency("USD");
        var targetCurrency = new Currency("EUR");
        var exchangeValue = 0.85m;
        var providerName = "TestProvider";
        var validUntil = DateTime.UtcNow.AddDays(1);

        var exchangeRate1 = new ExchangeRate(sourceCurrency, targetCurrency, exchangeValue, providerName, validUntil);
        var exchangeRate2 = new ExchangeRate(sourceCurrency, targetCurrency, exchangeValue, providerName, validUntil);

        Assert.Equal(exchangeRate1, exchangeRate2);
        Assert.True(exchangeRate1.Equals(exchangeRate2));
    }

    [Fact]
    public void ExchangeRate_Equals_ShouldReturnFalse_WhenDifferentProperties()
    {
        var sourceCurrency1 = new Currency("USD");
        var sourceCurrency2 = new Currency("EUR");
        var targetCurrency = new Currency("JPY");
        var exchangeValue = 0.85m;
        var providerName = "TestProvider";
        var validUntil = DateTime.UtcNow.AddDays(1);

        var exchangeRate1 = new ExchangeRate(sourceCurrency1, targetCurrency, exchangeValue, providerName, validUntil);
        var exchangeRate2 = new ExchangeRate(sourceCurrency2, targetCurrency, exchangeValue, providerName, validUntil);

        Assert.NotEqual(exchangeRate1, exchangeRate2);
        Assert.False(exchangeRate1.Equals(exchangeRate2));
    }

    [Fact]
    public void ExchangeRate_GetHashCode_ShouldReturnSameValue_WhenSameProperties()
    {
        var sourceCurrency = new Currency("USD");
        var targetCurrency = new Currency("EUR");
        var exchangeValue = 0.85m;
        var providerName = "TestProvider";
        var validUntil = DateTime.UtcNow.AddDays(1);

        var exchangeRate1 = new ExchangeRate(sourceCurrency, targetCurrency, exchangeValue, providerName, validUntil);
        var exchangeRate2 = new ExchangeRate(sourceCurrency, targetCurrency, exchangeValue, providerName, validUntil);

        Assert.Equal(exchangeRate1.GetHashCode(), exchangeRate2.GetHashCode());
    }

    [Fact]
    public void ExchangeRate_ToString_ShouldReturnFormattedString()
    {
        var sourceCurrency = new Currency("USD");
        var targetCurrency = new Currency("EUR");
        var exchangeValue = 0.85m;
        var providerName = "TestProvider";
        var validUntil = DateTime.UtcNow.AddDays(1);

        var exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, exchangeValue, providerName, validUntil);

        var result = exchangeRate.ToString();

        Assert.Contains("USD", result);
        Assert.Contains("EUR", result);
        Assert.Contains("0.85", result);
    }
} 