using ExchangeRateUpdater.Domain.Models;

namespace ExchangeRateUpdater.Tests.Shared.Builders;

public class ExchangeRateBuilder
{
    private Currency _sourceCurrency = new("TRY");

    private Currency _targetCurrency = new("CZK");

    private decimal _value = 1;

    public ExchangeRate Build()
    {
        return new ExchangeRate(_sourceCurrency, _targetCurrency, _value);
    }

    public ExchangeRateBuilder WithSourceCurrency(string code)
    {
        _sourceCurrency = new Currency(code);
        return this;
    }

    public ExchangeRateBuilder WithTargetCurrency(string code)
    {
        _targetCurrency = new Currency(code);
        return this;
    }

    public ExchangeRateBuilder WithValue(decimal value)
    {
        _value = value;
        return this;
    }
}