using ExchangeRateUpdater.Domain.Interfaces;

namespace ExchangeRateUpdater.Domain.Config
{
    public class CnbApiConfig : HttpClientBaseConfig
    {
        public string ExchangeRateApiUrl { get; set; }
        public string LocalCurrencyIsoCode { get; set; }
        public string PreferredLanguage { get; set; }
    }
}
