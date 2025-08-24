using Mews.ExchangeRateMonitor.ExchangeRate.Domain;

namespace Mews.ExchangeRateMonitor.ExchangeRate.Features.Features.GetExratesDaily;

public static class GetExratesDailyMappings
{
    public static CurrencyExchangeRate ToCurrencyExchangeRate(this CnbApiDailyRateDto cnbRate, string targerCurrency) => 
        new CurrencyExchangeRate(new(cnbRate.CurrencyCode), new(targerCurrency), cnbRate.Amount, cnbRate.Rate);

    public static CurrencyExchangeRateDto ToRateDto(this CurrencyExchangeRate rate) =>
        new(rate.SourceCurrency.Code, rate.TargetCurrency.Code, rate.SourceCurrencyAmount, rate.TargetCurrencyRate);
}

