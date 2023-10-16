namespace Mews.ExchangeRateUpdater.Services.ExternalRateProviders.CNB.Mapping
{
    /// <summary>
    /// This is the DTO class for capturing the error response from the external exchange rate provider
    /// </summary>
    public class ErrorResponse
    {
        public string Description { get; set; }

        public string EndPoint { get; set; }

        public string ErrorCode { get; set; }

        public string HappenedAt { get; set; }
    }
}
