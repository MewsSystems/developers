namespace ExchangeRates.Api.Infrastructure.Clients.Cnb.Models;

public class CnbExchangeRates
{
    public CnbExchangeRate[]? Rates { get; set; }
}

public class CnbExchangeRate
{
    public string ValidFor { get; set; } = default!;
    public int Order { get; set; }
    public string Country { get; set; } = default!;
    public string Currency { get; set; } = default!;
    public int Amount { get; set; }
    public string CurrencyCode { get; set; } = default!;
    public decimal Rate { get; set; }
}
