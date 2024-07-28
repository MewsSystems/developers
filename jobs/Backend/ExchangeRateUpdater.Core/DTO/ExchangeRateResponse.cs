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
        public decimal SourceValue { get; set; }
        public string? TargetCurrency { get; set; }
        public decimal TargetValue { get; set; }
    }

    public static class ExchangeRateExtensions
    {
        public static ExchangeRateResponse ToExchangeRateResponse(this ExchangeRate exchangeRate)
        {
            return new ExchangeRateResponse()
            {
                SourceCurrency = exchangeRate.SourceCurrency,
                SourceValue = exchangeRate.SourceValue,
                TargetCurrency = exchangeRate.TargetCurrency,
                TargetValue = exchangeRate.TargetValue
            };
        }
    }

}
