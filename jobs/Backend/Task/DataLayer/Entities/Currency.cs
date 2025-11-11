namespace DataLayer.Entities;

public class Currency
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<ExchangeRateProvider> ProvidersWithBaseCurrency { get; set; } = new List<ExchangeRateProvider>();
    public ICollection<ExchangeRate> BaseCurrencyRates { get; set; } = new List<ExchangeRate>();
    public ICollection<ExchangeRate> TargetCurrencyRates { get; set; } = new List<ExchangeRate>();
}
