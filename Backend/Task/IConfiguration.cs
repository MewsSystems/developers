namespace ExchangeRateUpdater
{
    public interface IConfiguration
    {
        string CNB_URL_MAIN { get; }
        string CNB_URL_OTHER { get; }
    }
}
