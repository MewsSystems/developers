using ExchangeRateUpdater.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Core.DTO.Client
{
    /// <summary>
    /// Response object used to map values back from the Czech National Bank API
    /// </summary>
    public class CNBExchangeRateResponse
    {
        public IEnumerable<CNBExchangeRate>? Rates { get; set; }
    }


    public class CNBExchangeRate()
    {
        public string? CurrencyCode { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public string? Country { get; set; }
    }

}
