using System.Configuration;

namespace ExchangeRateUpdater
{
    public class Configuration : IConfiguration
    {
        public string GetAppSettingValue(string name) => ConfigurationManager.AppSettings.Get("CnbApiUrl");
    }
}