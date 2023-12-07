namespace ExchangeRateUpdater.Host.WebApi.Tests.Unit.Dtos;

internal class ExchangeRateDto
{
    public string? From { get; set; }
    public string? To { get; set; }
    public decimal ExchangeRate { get; set; }
    public DateTime ExchangeRateTime { get; set; }
}

