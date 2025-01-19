using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Models.Requests
{
    public class ExchangeRateRequestDto
    {
        public DateTime Date { get; set; }
        public List<ExchangeRateRequest> ExchangeRatesDetails { get; set; }
    }
}
