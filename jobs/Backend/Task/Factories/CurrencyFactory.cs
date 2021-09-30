namespace ExchangeRateUpdater.Factories
{
    public static class CurrencyFactory
    {
        public static Currency Create(string code)
        {
            return new Currency(code);
        }
    }
}