namespace Mews.ExchangeRateUpdater.Services.ExternalRateProviders.CNB.Mapping
{
    /// <summary>
    /// This is the DTO class for capturing the response from the external exchange rate provider
    /// </summary>
    public class ExchangeRates
    {
        public IEnumerable<ExchangeRateDetails> Rates { get; set; }
    }
}
