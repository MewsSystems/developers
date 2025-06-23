namespace Domain.Models;

public sealed class RawExchangeRates
{ 
    public List<RawExchangeRate> Rates { get; set; }
}

public sealed record RawExchangeRate
{
    public string Country { get; set; }
    public string Currency { get; set; }
    public string CurrencyCode { get; set; }
    public decimal Rate { get; set; }
}