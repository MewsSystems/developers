namespace ExchangeRateUpdater.Models
{
    public class CzechNationalBankOptions
    {
        public string BaseAddress { get; set; }
        public string Endpoint { get; set; }
        
        /// <summary>
        /// The language used to list the countries in the response. 
        /// Defaults to Czech (CZ) as it's a Czech-based institution.
        /// </summary>
        public string Language { get; set; } = "CZ";
    }
}
