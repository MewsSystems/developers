using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Application.DTOs
{
    public record ExchangeRateDTO
    {
        public DateTime ValidFor { get; init; }
        public int Order { get; init; }
        public string Country { get; init; }
        public string Currency { get; init; }
        public int Amount { get; init; }
        public string CurrencyCode { get; init; }
        public decimal Rate { get; init; }
    }
}
