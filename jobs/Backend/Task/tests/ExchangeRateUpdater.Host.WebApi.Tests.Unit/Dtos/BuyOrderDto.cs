namespace ExchangeRateUpdater.Host.WebApi.Tests.Unit.Dtos;

internal class BuyOrderDto
{
    public string? SourceCurrency { get; set; }
    public string? TargetCurrency { get; set; }
    public decimal? SumToExchange { get; set; }
}
