using ExchangeRateUpdater.Domain.ValueObjects;

namespace ExchangeRateUpdater.Domain.Entities
{
    public class MonthYearExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value, DateOnly date) : ExchangeRate(sourceCurrency, targetCurrency, value)
    {
        public DateOnly Date { get; } = date;
        public override string ToString() => $"{Date:yyyy-MM-dd} | {base.ToString()}";
    }
}

