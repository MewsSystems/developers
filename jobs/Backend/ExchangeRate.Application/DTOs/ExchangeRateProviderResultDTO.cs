using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Application.DTOs
{
    public class ExchangeRateProviderResultDTO
    {
        public ExchangeRateProviderResultDTO(Dictionary<string, ExchangeRateProviderDTO> rates) { 
            Results = rates;
        }
        public Dictionary<string, ExchangeRateProviderDTO> Results {  get; set; }
    }
}
