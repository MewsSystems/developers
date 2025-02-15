namespace ExchangeRate.Application.DTOs
{
    public class ExchangeRatesDTO
    {
        public ExchangeRatesDTO(List<ExchangeRateBankDTO> exchangeRates, List<CurrencyDTO> currencies)
    {
        ExchangeRates = exchangeRates ?? new List<ExchangeRateBankDTO>();
        Currencies = currencies ?? new List<CurrencyDTO>();
    }
        /// <summary>
        /// List of exchange rates adhere currencies.
        /// </summary>
        public List<ExchangeRateBankDTO> ExchangeRates { get; set; } = new List<ExchangeRateBankDTO>();
        public List<CurrencyDTO> Currencies { get; set; } = new List<CurrencyDTO>();
    }
}