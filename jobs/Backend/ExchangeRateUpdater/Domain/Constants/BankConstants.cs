namespace ExchangeRateUpdater.Domain.Constants
{
    public class BankConstants
    {
        public class CzechNationalBank
        {
            public const string HttpClientIdentifier = "czech-exchange-rate-client";
            public const string DefaultCurrency = "CZK";
        }

        public class DeNederlandscheBank
        {
            public const string HttpClientIdentifier = "dutch-exchange-rate-client";
            public const string DefaultCurrency = "EUR";
        }
    }
}
