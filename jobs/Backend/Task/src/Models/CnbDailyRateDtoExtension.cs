using CnbApi.Models;

namespace ExchangeRateUpdater.Models;
public static class CnbDailyRateDtoExtensions
{
    public static ExchangeRate ToExchangeRate(this CnbDailyRateDto rateDto)
    {
        return new ExchangeRate(new Currency(rateDto.CurrencyCode), new Currency("CZK"), rateDto.Amount, rateDto.Rate);
    }
}
