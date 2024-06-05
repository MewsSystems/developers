namespace ExchangeRateProvider.BankApiClients.Cnb.Models;

public class CnbBankResponse
{
	public IEnumerable<CnbBankCurrencyRate>? Rates { get; set; }
}
