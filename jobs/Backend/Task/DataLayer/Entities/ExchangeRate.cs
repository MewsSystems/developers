namespace DataLayer.Entities;

public class ExchangeRate
{
    public int Id { get; set; }
    public int ProviderId { get; set; }
    public int BaseCurrencyId { get; set; }
    public int TargetCurrencyId { get; set; }
    public int Multiplier { get; set; }
    public decimal Rate { get; set; }
    public DateOnly ValidDate { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset? Modified { get; set; }

    // Navigation properties
    public ExchangeRateProvider Provider { get; set; } = null!;
    public Currency BaseCurrency { get; set; } = null!;
    public Currency TargetCurrency { get; set; } = null!;
}
