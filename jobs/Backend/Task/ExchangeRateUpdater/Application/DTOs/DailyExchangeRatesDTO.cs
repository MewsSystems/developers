using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Application.DTOs
{
    public record DailyExchangeRatesDTO
    {
        public IEnumerable<ExchangeRateDTO> Rates { get; init; }
    }
}
