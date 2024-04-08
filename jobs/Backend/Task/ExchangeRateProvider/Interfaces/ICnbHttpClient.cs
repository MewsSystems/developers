using ExchangeRateProvider.Constants;
using ExchangeRateProvider.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateProvider.Interfaces
{
    public interface ICnbHttpClient
    {
        Task<CnbRatesModel?> GetCzkExchangeRatesAsync(DateTime dateTime, string language = Language.Czech, CancellationToken cancellationToken = default);
    }
}