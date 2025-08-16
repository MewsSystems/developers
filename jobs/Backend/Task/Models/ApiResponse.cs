using System.Collections.Generic;

namespace ExchangeRateUpdater.Models
{
    public class ApiResponse
    {
        public List<ApiExchangeRate> Rates { get; set; }
    }
}
