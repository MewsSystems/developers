using System.Collections.Generic;

namespace ExchangeRateUpdater.ExchangeRateAPI.DTOs
{
    public class ExchangeRatesResponseDTO
    {
        public List<RateDTO> Rates { get; set; }
    }
}
