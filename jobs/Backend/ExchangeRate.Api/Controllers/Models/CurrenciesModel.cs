using ExchangeRate.Api.Validators;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ExchangeRate.Api.Models
{
    /// <summary>
    /// List of Currencies class.
    /// </summary>
    public class CurrenciesModel
    {
        /// <summary>
        /// List of currency codes to be retrieved comparing with 'CZK'.
        /// </summary>
        [NotNull]
        [JsonPropertyName("currencyCodes")]
        public List<CurrencyModel> CurrencyCodes { get; set; } = new();

        /// <summary>
        /// Day to retrieve currency codes.
        /// </summary>
        [NotNull]
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
    }
}