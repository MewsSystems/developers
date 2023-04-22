namespace ExchangeRateUpdater
{
    public class ExchangeRate
    {
        public ExchangeRate(Currency currency, decimal value)
        {
            CurrencyCode = currency;
            CurrencyValue = value;
        }

        public Currency CurrencyCode { get; }

        public decimal CurrencyValue { get; }

        public override string ToString()
        {
            return $"{CurrencyCode}={CurrencyValue}";
        }
    }
}
