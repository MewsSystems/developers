namespace ExchangeRateUpdater
{
    public class ConfigurationSettings
    {
        public const string CNBWebsiteUrl = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/";
        public static string GetCNBWebsiteURL()
        {
            return CNBWebsiteUrl;
        }
       
    }
}
