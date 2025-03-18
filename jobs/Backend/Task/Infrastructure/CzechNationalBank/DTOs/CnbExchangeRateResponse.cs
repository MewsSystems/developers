using System.Collections.Generic;

namespace ExchangeRateUpdater.Infrastructure.CzechNationalBank.DTOs
{
    /// <summary>
    /// Represents the response from the Czech National Bank API containing exchange rates.
    /// </summary>
    public class CnbExchangeRateResponse
    {
        public List<CnbRate> Rates { get; set; }
    }
}
