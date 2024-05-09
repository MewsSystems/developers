namespace ExchangeRateUpdater.Core.Configuration
{
    public class CzechNationalBankConfiguration
    {
        public Uri? ApiBaseUrl { get; set; }

        public string DefaultCurrencyCode { get; set;} = string.Empty;

        public int DecimalPlaces { get; set; }
    }
}
