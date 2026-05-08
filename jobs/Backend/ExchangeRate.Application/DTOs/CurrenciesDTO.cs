using System.ComponentModel.DataAnnotations;

namespace ExchangeRate.Application.DTOs
{
    public class CurrenciesDTO
    {
        public IEnumerable<CurrencyDTO> CurrencyCodes { get; set; } = new List<CurrencyDTO>();
        public DateTime Date { get; }
    }
}