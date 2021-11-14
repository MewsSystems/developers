namespace ExchangeRateUpdater
{
    public interface IExchangeRatesSource
    {
        ExchangeRate Get(ExchangePair currencyPair);
    }
}