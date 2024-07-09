namespace ExchangeRateUpdater.Infrastructure.Clients.CzechNationalBank;

public class GetCzechNationalBankExchangeRatesResponse
{
    public IEnumerable<CzechNationalBankExchangeRate> Rates { get; set; } = default!;
}
