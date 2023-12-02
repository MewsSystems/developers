namespace ExchangeRateUpdater.Host.WebApi.Tests.Unit.Dtos
{
    public class BuyResultDto
    {
        public string? SourceCurrency { get; set; }
        public string? TargetCurrency { get; set; }

        public decimal ConvertedSum { get; set; }
    }
}
