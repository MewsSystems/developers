using ExchangeRateUpdater.Cnb.Dtos;

namespace ExchangeRateUpdater
{
    public class CnbMapper
    {
        public Model.ExchangeRate MapExchangeRate(ExchangeRate exchangeRate)
        {
            return new Model.ExchangeRate(
                new Model.Currency(exchangeRate.SourceCurrencyCode),
                new Model.Currency(exchangeRate.TargetCurrencyCode),
                exchangeRate.Rate / exchangeRate.Amount);
        }
    }
}
