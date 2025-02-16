namespace ExchangeRate.Application.DTOs
{
    public class ExchangeRatesBankDTO
    {
        public ExchangeRatesBankDTO(List<ExchangeRateBankDTO> exchangeRates)
        {
            ExchangeRates = exchangeRates ?? new List<ExchangeRateBankDTO>();
        }
        /// <summary>
        /// List of exchange rates.
        /// </summary>
        public List<ExchangeRateBankDTO> ExchangeRates { get; set; } = new List<ExchangeRateBankDTO>();
        
    }
}