namespace ExchangeRateUpdater.Host.WebApi.Dtos.Response
{
    public class BuyResultDto
    {
        public string? SourceCurrency { get; set; }
        public string? TargetCurrency { get; set; }

        public decimal ConvertedSum { get; set; }
    }
}
