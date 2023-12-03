namespace ExchangeRateUpdater.Host.WebApi.Dtos.Response
{
    public class ExchangeResultDto
    {
        public string? SourceCurrency { get; set; }
        public string? TargetCurrency { get; set; }

        public decimal ConvertedSum { get; set; }
    }
}
