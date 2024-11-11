using Application.Common.Models;
using Application.CzechNationalBank.ApiClient.Dtos;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.CzechNationalBank.Mappings
{
    public class CNBExchangeRateMappingService(ILogger<CNBExchangeRateMappingService> _logger) : ICNBExchangeRateMappingService
    {
        public static string CzechCurrencyCode => "CZK";

        // would normally use an extension method or perhaps automapper but need to handle case where Amount is 0
        public IList<ExchangeRate> ConvertToExchangeRates(IEnumerable<CNBExRateDailyRestDto> dtos)
        {
            var results = new List<ExchangeRate>();
            foreach (var dto in dtos)
            {
                if (dto.Amount <= 0)
                {
                    _logger.LogWarning("Amount is 0 for currency code {CurrencyCode}. Skipping.", dto.CurrencyCode);
                    continue;
                }
                var sourceCurrency = new Currency(dto.CurrencyCode);
                var targetCurrency = new Currency(CzechCurrencyCode);
                var rate = dto.Rate / dto.Amount;
                results.Add(new ExchangeRate(sourceCurrency, targetCurrency, rate));
            }
            return results;
        }
    }
}
