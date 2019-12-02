using System.Globalization;

namespace ExchangeRateUpdater
{
    public class RateEntry
    {
        public Currency Currency { get; }
        public int Unit { get; }
        public decimal Rate { get; }

        public RateEntry(Currency currency, int unit, decimal rate)
        {
            Currency = currency;
            Unit = unit;
            Rate = rate;
        }

        public ExchangeRate ToExchangeRate(Currency sourceCurrency, Currency targetCurrency)
        {
            return new ExchangeRate(sourceCurrency, targetCurrency, Rate / Unit);
        }

        public static RateEntry Parse(string source)
        {
            var fields = source.Split('|');
            return new RateEntry(new Currency(fields[3]), int.Parse(fields[2]), decimal.Parse(fields[4], NumberFormatInfo.CurrentInfo));
        }
    }
}