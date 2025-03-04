using ExchangeRateUpdater.Dto.ExchangeRates;
using ExchangeRateUpdater.Infrastructure.CNB.Entities;

namespace ExchangeRateUpdater.Business.ExchangeRates.Extensions;

public static class ExchangeRatesMapper
{
    public static ExchangeRate ToExchangeRate(this CnbExchangeRateEntity cnbExchangeRateEntity)
        => new(
            new Currency(ExchangeRateConstants.SourceCurrencyExchangeRate),
            new Currency(cnbExchangeRateEntity.CurrencyCode),
            cnbExchangeRateEntity.Rate);
}