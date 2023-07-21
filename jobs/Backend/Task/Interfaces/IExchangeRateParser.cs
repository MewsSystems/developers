using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Interfaces
{
    public interface IExchangeRateParser
    {
        ExchangeRatesResponse ParseRatesFromApi(string response);
        List<ExchangeRate> ParseRatesFromText(IEnumerable<Currency> currencies, string[] lines);
        List<ExchangeRate> GetRatesFromData(IEnumerable<Currency> currencies, ExchangeRatesResponse data);
    }

}
