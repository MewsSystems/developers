namespace ExchangeRateUpdater
{
    public interface ICnbClient
    {
        (bool Success, string RawData) GetRawDailyExchanges();
    }
}
