using ExchangeRateFinder.ConsoleApp.Responses.Models;
using System.Collections.Generic;

namespace ExchangeRateFinder.ConsoleApp.Responses
{
    public class CalculatedExchangeRateResponse
    {
        public List<CalculatedExchangeRate> ExchangeRates { get; set; }
    }
}
