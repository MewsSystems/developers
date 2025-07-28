namespace Mews.ExchangeRateUpdater.Infrastructure.Persistance.Models;

public class ExchangeRateEntity
{
    public DateTime Date { get; set; }
    public string SourceCurrency { get; set; } = default!;
    public string TargetCurrency { get; set; } = default!;
    public decimal Value { get; set; }
}