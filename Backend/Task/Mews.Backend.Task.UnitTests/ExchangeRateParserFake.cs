using Mews.Backend.Task.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mews.Backend.Task.UnitTests
{
    public class ExchangeRateParserFake : IExchageRateParser
    {
        public Task<IEnumerable<ExchangeRateDto>> GetExchangeRatesAsync()
        {
            var exchangeRateDtos = new List<ExchangeRateDto>
            {
                new ExchangeRateDto
                {
                    Country = "Turkey",
                    Currency = "lira",
                    Amount = 1,
                    Code = "TRY",
                    Rate = 3.908m
                },
                new ExchangeRateDto
                {
                    Country = "United Kingdom",
                    Currency = "pound",
                    Amount = 1,
                    Code = "GBP",
                    Rate = 28.839m
                },
                new ExchangeRateDto
                {
                    Country = "USA",
                    Currency = "dollar",
                    Amount = 1,
                    Code = "USD",
                    Rate = 22.623m
                }
            };

            return System.Threading.Tasks.Task.FromResult(exchangeRateDtos.AsEnumerable());
        }
    }
}
