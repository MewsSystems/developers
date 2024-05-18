namespace ExchangeRateUpdater.Configuration
{
    public class CnbApiClientConfiguration
    {
        public string ApiBaseUri { get; set; }

        /*
         * Note: Normally there would be other API specific properties here
         * for example ClientId and ClientSecret
         * or if it's communication between Azure App Services, an IdentifierUri
         */

        public string BaseCurrency { get; set; }
    }
}
