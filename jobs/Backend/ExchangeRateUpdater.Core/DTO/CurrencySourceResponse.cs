using ExchangeRateUpdater.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Core.DTO
{
    public class CurrencySourceResponse
    {
        public Guid CurrencySourceId { get; set; }
        public string? CurrencyCode { get; set; }
        public string? SourceUrl { get; set; }
    }

    public static class CurrencySourceExtensions
    {
        public static CurrencySourceResponse ToCurrencySourceResponse(this CurrencySource currencySource)
        {
            return new CurrencySourceResponse()
            {
                CurrencySourceId = currencySource.Id,
                CurrencyCode = currencySource.CurrencyCode,
                SourceUrl = currencySource.SourceUrl
            };
        }
    }
}
