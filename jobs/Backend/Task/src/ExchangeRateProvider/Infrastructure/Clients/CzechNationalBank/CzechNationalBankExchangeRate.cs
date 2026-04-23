namespace ExchangeRateUpdater.Infrastructure.Clients.CzechNationalBank;

public class CzechNationalBankExchangeRate
{
    public ushort Amount { get; set; }
    public string Country { get; set; } = default!;
    public string Currency { get; set; } = default!;
    public string CurrencyCode { get; set; } = default!;
    public ushort Order { get; set; }
    public decimal Rate { get; set; }
    public DateOnly ValidFor {  get; set; }
}
