using ExchangeRateUpdater.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Core.DTO
{
    public class ExchangeRateResponse
    {
        public string? SourceCurrency { get; set; }
        public string? TargetCurrency { get; set; }
        public decimal Value { get; set; }
    }

    public static class ExchangeRateExtensions
    {
        public static ExchangeRateResponse ToExchangeRateResponse(this ExchangeRate exchangeRate)
        {
            return new ExchangeRateResponse()
            {
                SourceCurrency = exchangeRate.SourceCurrency,
                TargetCurrency = exchangeRate.TargetCurrency,
                Value = exchangeRate.Value
            };
        }
    }

}
