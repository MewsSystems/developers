using ExchangeRateUpdater.Clients.Cnb.Responses;

namespace ExchangeRateUpdater.Tests.Shared.Builders;

public class ExchangeRateDtoBuilder
{
    private string _country = "Turkey";
    private string _currency = "lira";
    private int _amount = 1;
    private string _code = "USD";
    private decimal _rate = (decimal)25.298;

    public ExchangeRateDto? Build()
    {
        return new ExchangeRateDto
        {
            Code = _code,
            Country = _country,
            Amount = _amount,
            Rate = _rate,
            Currency = _currency
        };
    }

    public ExchangeRateDtoBuilder WithCode(string code)
    {
        _code = code;
        return this;
    }

    public ExchangeRateDtoBuilder WithCountry(string country)
    {
        _country = country;
        return this;
    }

    public ExchangeRateDtoBuilder WithAmount(int amount)
    {
        _amount = amount;
        return this;
    }

    public ExchangeRateDtoBuilder WithRate(decimal rate)
    {
        _rate = rate;
        return this;
    }

    public ExchangeRateDtoBuilder WithCurrency(string currency)
    {
        _currency = currency;
        return this;
    }
}