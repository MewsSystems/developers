namespace ExchangeRate.Client.Cnb.Models.Txt
{
	/// <summary>
	/// Exchange rate model for TXT api
	/// </summary>
	public class TxtExchangeRate
	{
		public TxtExchangeRate(string country, string currency, int amount, string code, decimal rate)
		{
			Country = country;
			Currency = currency;
			Amount = amount;
			Code = code;
			Rate = rate;
		}

		public string Country { get; set; }
		public string Currency { get; set; }
		public int Amount { get; set; }
		public string Code { get; set; }
		public decimal Rate { get; set; }
	}
}
