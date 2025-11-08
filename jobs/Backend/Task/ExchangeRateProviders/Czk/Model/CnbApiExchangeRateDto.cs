namespace ExchangeRateProviders.Czk.Model
{
	public class CnbApiExchangeRateDto
	{
		public DateTime ValidFor { get; set; }
		public int Amount { get; set; }
		public string CurrencyCode { get; set; } = string.Empty;
		public decimal Rate { get; set; }
	}
}
