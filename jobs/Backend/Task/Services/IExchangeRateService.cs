using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateProvider.Models;

namespace ExchangeRateProvider.Services;

public interface IExchangeRateService
{
    Task<IEnumerable<ExchangeRate>> GetCurrencyExchangeRatesAsync(string targetCurrencyCode);
}