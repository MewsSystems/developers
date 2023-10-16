namespace Mews.ExchangeRateUpdater.Services.Models
{
    public class CurrencyModel
    {
        /// <summary>
        /// The name of the currency 
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; set; }
    }
}
