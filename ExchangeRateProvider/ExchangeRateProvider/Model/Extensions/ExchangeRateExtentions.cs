using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateProvider.Model.Extensions
{
    /// <summary>
    /// ExchangeRateExtentions - <c>nordic</c> bank currency api projections
    /// </summary>
    public static class ExchangeRateExtentions
    {
        public static IEnumerable<ExchangeRateDto> AsExchangeRateEnumerable(this IEnumerable<ExchangeRateEntry> entries, string bankCode = "NOK")
        {
            return entries.AsEnumerable()?.Select(tableEntry =>
                new ExchangeRateDto
                {
                    SourceCurrency = new CurrencyDto { Code = tableEntry?.Id },
                    TargetCurrency = new CurrencyDto { Code = bankCode },
                    Value = tableEntry.CurrentValue
                });
        }
    }

}
