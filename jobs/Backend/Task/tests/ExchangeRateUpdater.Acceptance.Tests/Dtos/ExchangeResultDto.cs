namespace ExchangeRateUpdater.Acceptance.Tests.Dtos;

internal class ExchangeResultDto
{
    public string? SourceCurrency { get; set; }
    public string? TargetCurrency { get; set; }

    public decimal ConvertedSum { get; set; }
}
