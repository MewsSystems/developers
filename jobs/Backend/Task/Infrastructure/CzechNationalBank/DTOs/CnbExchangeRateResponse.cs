using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
