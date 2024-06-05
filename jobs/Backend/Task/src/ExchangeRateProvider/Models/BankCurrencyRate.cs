namespace ExchangeRateProvider.Models;

public class BankCurrencyRate
{
	public required long Amount { get; set; }
	public required string CurrencyCode { get; set; }
	public required decimal Rate { get; set; }
}


