using System.Configuration;

namespace ExchangeRateUpdater
{
    public class Configuration : IConfiguration
    {
        public string CNB_URL_MAIN => ConfigurationManager.AppSettings["CNB_URL_MAIN"]?.ToString();
        public string CNB_URL_OTHER => ConfigurationManager.AppSettings["CNB_URL_OTHER"]?.ToString();
    }
}
