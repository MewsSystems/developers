using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Domain.Entities
{
	public class ExternalCurrencyRateResponse
	{
		[JsonPropertyName("rates")]
		public List<ExternalCurrencyRate> Rates { get; set; }
	}
}
