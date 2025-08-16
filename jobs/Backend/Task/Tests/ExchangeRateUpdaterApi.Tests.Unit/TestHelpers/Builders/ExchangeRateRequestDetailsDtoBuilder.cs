using ExchangeRateUpdaterApi.Tests.Unit.Dtos.Request;

namespace ExchangeRateUpdaterApi.Tests.Unit.TestHelpers.Builders;

public class ExchangeRateRequestDetailsDtoBuilder
{
    private CurrencyDto _sourceCurrency;
    private CurrencyDto _targetCurrency;

    internal ExchangeRateRequestDetailsDtoBuilder()
    { }

    public ExchangeRateRequestDetailsDto Build()
    {
        return new ExchangeRateRequestDetailsDto
        {
            SourceCurrency = _sourceCurrency,
            TargetCurrency = _targetCurrency,
        };
    }

    public ExchangeRateRequestDetailsDtoBuilder WithSourceCurrency(CurrencyDto value)
    {
        _sourceCurrency = value;
        return this;
    }

    public ExchangeRateRequestDetailsDtoBuilder WithTargetCurrency(CurrencyDto value)
    {
        _targetCurrency = value;
        return this;
    }
}