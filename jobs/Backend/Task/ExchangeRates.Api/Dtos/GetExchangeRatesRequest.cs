using ExchangeRates.Api.Dtos;

namespace ExchangeRates.Api.DTOs
{
    public class GetExchangeRatesRequest
    {
        [ValidCurrencyCodes]
        public IEnumerable<string>? Currencies { get; set; }
    }
}
