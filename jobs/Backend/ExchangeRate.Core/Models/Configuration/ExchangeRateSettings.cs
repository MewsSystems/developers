namespace ExchangeRates.Core.Models.Configuration
{
    /// <summary>
    /// Application configuration settings
    /// </summary>
    public class ExchangeRateSettings
    {
        public string CnbApiUrl { get; set; } = string.Empty;
        public string BaseCurrency { get; set; } = string.Empty;
    }
}