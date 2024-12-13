using ExchangeRateUpdater.Domain.ValueObjects;

namespace ExchangeRateUpdater.Domain.Entities
{
    public class DailyExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value) : ExchangeRate(sourceCurrency, targetCurrency, value)
    {

    }
}

