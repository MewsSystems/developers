namespace ExchangeRateUpdater.CzechNationalBank.Configuration
{
    public class ApiClientConfiguration
    {
        public const string HttpProtocol = "https://";
        public const string DomainUrl = "www.cnb.cz";

        public const string ExchangeRatePath =
            "/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";
    }
}