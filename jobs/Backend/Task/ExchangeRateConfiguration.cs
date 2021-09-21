using ExchangeRateUpdater.Configuration;
using System.Configuration;

namespace ExchangeRateUpdater
{
    public class ExchangeRateConfiguration : IExchangeRateConfiguration
    {
        public string Url => ConfigurationManager.AppSettings["cnbUrl"];
    }
}
