namespace ExchangeRate.Application.DTOs
{
    public class CurrenciesBankDTO
    {
        public CurrenciesBankDTO(List<CurrencyDTO> currencies)
        {
            this.Currencies = currencies;
        }
        public List<CurrencyDTO> ToList()
        {
            return Currencies;
        }
        /// <summary>
        /// List of Currency codes.
        /// </summary>
        private List<CurrencyDTO> Currencies { get; set; } = new List<CurrencyDTO>();
        public DateTime Date { get; }

    }
}