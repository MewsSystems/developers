namespace Mews.ERP.AppService;

public class Constants
{
    public struct FetchFeature
    {
        public const int RestMaxRetryCount = 3;
    }
    
    public struct CzechNationalBank
    {
        public const string CzechCurrencyCode = "CZK";

        public const string CzechApiRoute = "https://api.cnb.cz/cnbapi";

        public const string ExchangeRatesDailyApiRoute = "exrates/daily";

        public const string EnglishLanguageIso2Code = "EN";
    }
}