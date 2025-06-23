namespace ExchangeRateUpdater.Api.Dtos
{
    /// <summary>
    /// Represents exchange rate information returned by the API.
    /// </summary>
    public class ExchangeRateDto
    {
        public string? SourceCurrency { get; set; }
        public string? TargetCurrency { get; set; }
        public decimal Rate { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
