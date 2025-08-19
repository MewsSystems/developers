using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services.Builders;
using FluentAssertions;

namespace ExchangeRateUpdater.Tests.Builders;

public class ExchangeRateBuilderTests
{
    private readonly ExchangeRateBuilder _builder;

    public ExchangeRateBuilderTests()
    {
        _builder = new ExchangeRateBuilder();
    }

    [Fact]
    public void BuildExchangeRates_WithMatchingCurrencies_ReturnsCorrectRates()
    {
        // Arrange
        var currencies = new List<Currency>
        {
            new Currency("EUR"),
            new Currency("USD"),
            new Currency("CZK")
        };

        var cnbData = new CnbExchangeRateData
        {
            Date = DateTime.Now,
            Rates = new List<CnbExchangeRateEntry>
            {
                new CnbExchangeRateEntry { Code = "EUR", Rate = 24.455m, Amount = 1 },
                new CnbExchangeRateEntry { Code = "USD", Rate = 20.927m, Amount = 1 }
            }
        };

        // Act
        var result = _builder.BuildExchangeRates(currencies, cnbData).ToList();

        // Assert
        result.Should().HaveCount(2);

        var eurToCzk = result.Single(r => r.SourceCurrency.Code == "EUR");
        eurToCzk.TargetCurrency.Code.Should().Be("CZK");
        eurToCzk.Value.Should().Be(24.455m);

        var usdToCzk = result.Single(r => r.SourceCurrency.Code == "USD");
        usdToCzk.TargetCurrency.Code.Should().Be("CZK");
        usdToCzk.Value.Should().Be(20.927m);
    }

    [Fact]
    public void BuildExchangeRates_WithAmountGreaterThanOne_CalculatesCorrectRate()
    {
        var currencies = new List<Currency> { new Currency("JPY"), new Currency("CZK") };
        var cnbData = new CnbExchangeRateData
        {
            Date = DateTime.Now,
            Rates = new List<CnbExchangeRateEntry>
            {
                new CnbExchangeRateEntry
                {
                    Code = "JPY",
                    Rate = 14.165m,
                    Amount = 100,
                    Country = "Japan",
                    Currency = "yen"
                }
            }
        };

        // Act
        var result = _builder.BuildExchangeRates(currencies, cnbData).ToList();

        // Assert
        result.Should().HaveCount(1);
        result[0].Value.Should().Be(0.14165m);
    }

    [Fact]
    public void BuildExchangeRates_WithCzkCurrency_ReturnsNoRate()
    {
        // Arrange
        var currencies = new List<Currency> { new Currency("CZK") };
        var cnbData = new CnbExchangeRateData
        {
            Date = DateTime.Now,
            Rates = new List<CnbExchangeRateEntry>
            {
                new CnbExchangeRateEntry { Code = "EUR", Rate = 24.455m, Amount = 1 }
            }
        };

        // Act
        var result = _builder.BuildExchangeRates(currencies, cnbData).ToList();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void BuildExchangeRates_WithEmptyCurrencyList_ReturnsEmpty()
    {
        // Arrange
        var currencies = new List<Currency>();
        var cnbData = new CnbExchangeRateData
        {
            Date = DateTime.Now,
            Rates = new List<CnbExchangeRateEntry>
            {
                new CnbExchangeRateEntry { Code = "EUR", Rate = 24.455m, Amount = 1 }
            }
        };

        // Act
        var result = _builder.BuildExchangeRates(currencies, cnbData);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void BuildExchangeRates_WithNullCurrencies_ThrowsArgumentNullException()
    {
        // Arrange
        var cnbData = new CnbExchangeRateData
        {
            Date = DateTime.Now,
            Rates = new List<CnbExchangeRateEntry>()
        };

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => _builder.BuildExchangeRates(null, cnbData).ToList());

        exception.ParamName.Should().Be("source");
    }

    [Fact]
    public void BuildExchangeRates_WithNullCnbData_ThrowsNullReferenceException()
    {
        // Arrange
        var currencies = new List<Currency> { new Currency("EUR") };

        // Act & Assert
        Assert.Throws<NullReferenceException>(() => _builder.BuildExchangeRates(currencies, null).ToList());
    }
}
