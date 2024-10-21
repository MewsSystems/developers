
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Entities
{
	public class ExchangeRatesDto
	{
		[JsonPropertyName("rates")]
		public List<ExchangeRateEntity> ExchangeRates { get; set; }
	}
}
