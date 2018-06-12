using System.Configuration;

namespace ExchangeRateUpdater
{
    public static class Configuration
    {
        private static string _cnbExchangeRatesUrl;

        public static string CnbExchangeRatesUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_cnbExchangeRatesUrl))
                {
                    _cnbExchangeRatesUrl = ConfigurationManager.AppSettings["CnbExchangeRatesUrl"];
                }

                return _cnbExchangeRatesUrl;
            }
        }
    }
}