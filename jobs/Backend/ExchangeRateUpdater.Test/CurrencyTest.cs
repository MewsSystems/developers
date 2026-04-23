namespace ExchangeRateUpdater.Test;

public class CurrencyTest
{
    [Fact]
    public void Currency_RecordValueComparison_ReturnsTrueForEqualCurrencies()
    {
        var symbol = "USD";
        var currency1 = new Currency(symbol);
        var currency2 = new Currency(symbol);

        var result = currency1.Equals(currency2);

        Assert.True(result);
    }

    [Fact]
    public void Currency_RecordValueComparison_ReturnsFalseForDifferentCurrencies()
    {
        var currency1 = new Currency("USD");
        var currency2 = new Currency("EUR");

        var result = currency1.Equals(currency2);

        Assert.False(result);
    }
}