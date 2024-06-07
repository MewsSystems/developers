namespace ExchangeRateProvider.BankApiClients.Cnb.Models;

public record CnbBankResponse
{
	public IEnumerable<CnbBankCurrencyRate>? Rates { get; set; }
}
