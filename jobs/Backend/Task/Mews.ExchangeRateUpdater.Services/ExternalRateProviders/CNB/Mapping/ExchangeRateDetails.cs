namespace Mews.ExchangeRateUpdater.Services.ExternalRateProviders.CNB.Mapping
{
    public class ExchangeRateDetails
    {
        public string ValidFor { get; set; }

        public string Country { get; set; }

        public string Currency { get; set; }

        public int Amount { get; set; }

        public string CurrencyCode { get; set; }

        public decimal Rate { get; set; }
    }
}
