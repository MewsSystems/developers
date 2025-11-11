namespace ExchangeRateUpdater.Common.Constants
{
    public static class ErpPlatform
    {
        public const string CnbEndpoint = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";
        public const string CnbSourceCurrency = "CZK";
        public const int DefaultTimeoutMs = 5000;
    }
}
