using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Entities
{
	public class ExchangeRateEntity
	{
			[JsonPropertyName("currencyCode")]
			public string CurrencyCode { get; set; }
			[JsonPropertyName("rate")]
			public decimal Rate { get; set; }
	}
}
