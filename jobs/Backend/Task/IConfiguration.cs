namespace ExchangeRateUpdater
{
    public interface IConfiguration
    {
        string GetAppSettingValue(string name);
    }
}