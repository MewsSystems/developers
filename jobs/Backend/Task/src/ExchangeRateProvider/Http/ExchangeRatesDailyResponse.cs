namespace ExchangeRateProvider.Http;

public class CurrencyRate
{
	public string? ValidFor { get; set; }
	public int? Order { get; set; }
	public string? Country { get; set; }
	public string? Currency { get; set; }
	public int? Amount { get; set; }
	public string? CurrencyCode { get; set; }
	public decimal? Rate { get; set; }
}

public class ExchangeRatesDailyResponse
{
	public IEnumerable<CurrencyRate>? Rates { get; set; }
}