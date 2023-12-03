namespace ExchangeRateUpdater.Host.WebApi.Dtos.Response;

internal class ExchangeRateDto
{
    public string? From { get; set; }
    public string? To { get; set; }
    public decimal ExchangeRate { get; set; }
}
