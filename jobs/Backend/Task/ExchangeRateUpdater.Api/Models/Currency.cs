using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Api.Models
{
    public class Currency
    {
        public Currency()
        {
        }

        public Currency(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException(nameof(code));
            }

            Code = code;
        }

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        public override string ToString()
        {
            return Code;
        }
    }
}
