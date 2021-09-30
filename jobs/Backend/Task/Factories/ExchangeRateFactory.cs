namespace ExchangeRateUpdater.Factories
{
    public static class ExchangeRateFactory
    {
        public static ExchangeRate CreateAsTargetCzk(string code, decimal value)
        {
            return new ExchangeRate(CurrencyFactory.Create("CZK"), CurrencyFactory.Create(code), value);
        }
    }
}