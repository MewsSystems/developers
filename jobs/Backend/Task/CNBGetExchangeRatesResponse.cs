namespace ExchangeRateUpdater
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class CNBGetExchangeRatesResponse
    {
        [JsonPropertyName("rates")]
        public List<CNBExchangeRate> Rates { get; set; }
    }
}
